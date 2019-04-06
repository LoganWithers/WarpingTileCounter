namespace WarpingCounter.Common
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Gadgets;

    using Models;

    public class BinaryReader
    {

        private readonly bool carry;

        private readonly int counterBase;

        private readonly (string guessOne, string guessZero) guesses;


        private readonly int index;

        private readonly string inputBits;

        public readonly List<Tile> Tiles;

        private readonly GuessOne guessOne;

        private readonly GuessZero guessZero;


        public BinaryReader(string inputBits, bool carry, int index, int length, int counterBase)
        {
            this.inputBits   = inputBits;
            this.index       = index;
            this.carry       = carry;
            this.counterBase = counterBase;

            guessOne  = new GuessOne(this.inputBits, this.carry, this.index);
            guessZero = new GuessZero(this.inputBits, this.carry, this.index);

            guessOne.Bind(guessZero);

            guesses = (guessOne.Result, guessZero.Result);

            var input = GetInputGlue(this.inputBits, this.carry, this.index);

            var (one, zero) = GetOutputGlues(length);

            guessZero.Last.North = zero;
            guessOne.Last.North  = one;
            guessOne.First.South = input;

            Tiles = new List<Tile>();
            Tiles.AddRange(guessOne.Tiles);
            Tiles.AddRange(guessZero.Tiles);

            if (inputBits.Length + 1 == length && inputBits.All(bit => bit == '1'))
            {
                guessOne.Last.North.Bind  = 0;
                guessZero.Last.North.Bind = 0;
            }
        }


        private static Glue GetInputGlue(string bits, bool carry, int index) => GlueFactory.DigitReader(bits, carry, index);


        private (Glue one, Glue zero) GetOutputGlues(int maxLength)
        {
            var valueProducedIsLessThanMax = inputBits.Length + 1 < maxLength;

            if (valueProducedIsLessThanMax)
            {
                return (GetInputGlue(guesses.guessOne,  carry, index),
                        GetInputGlue(guesses.guessZero, carry, index));
            }

            var one  = CalculateOutputBits(guesses.guessOne,  carry, counterBase);
            var zero = CalculateOutputBits(guesses.guessZero, carry, counterBase);

            return (GlueFactory.PreFirstWarp(one.bits,  one.carryOut,  index),
                    GlueFactory.PreFirstWarp(zero.bits, zero.carryOut, index));
        }


        [Pure]
        private static (string bits, bool carryOut) CalculateOutputBits(string inputBits, bool carry, int baseM)
        {
            const int binary = 2;

            if (!carry)
            {
                return (inputBits, false);
            }

            // Right most bits, do not impact the value that used by the counter. 
            var tail = inputBits.GetLast(2);

            // All bits but the tail.
            var bits = inputBits.Substring(0, inputBits.Length - 2);

            // Convert the first n - 2 bits to an integer. 
            var value = Convert.ToInt32(bits, binary);

            bool CanAddOne(int n) => n + 1 <= baseM - 1;

            if (CanAddOne(value))
            {
                var incremented = Convert.ToString(value + 1, binary)
                                         .PadLeft(bits.Length, '0');

                // carry signal goes to false since we incremented, 
                // the new value is the increments bits, with the original digit/region indicators 
                // re-added to the end
                return ($"{incremented}{tail}", false);
            }

            // set all the bits to 0 since adding a value to the current value will 
            // result in some value that is not less than base M. Propagate the 
            // carry signal
            var carryOut = string.Concat(Enumerable.Repeat("0", bits.Length));

            return ($"{carryOut}{tail}", true);
        }


        private class GuessOne : IHaveFirst, IHaveLast
        {

            private readonly Tile first;

            private readonly Tile fourth;

            private readonly Tile second;

            private readonly Tile third;

            public readonly List<Tile> Tiles;


            public GuessOne(string inputBits, bool carry, int index)
            {
                first  = new Tile(Name(inputBits, carry, index));
                second = new Tile(Name(inputBits, carry, index));
                third  = new Tile(Name(inputBits, carry, index));
                fourth = new Tile(Name(inputBits, carry, index));

                First  = first;
                Last   = fourth;
                Result = $"1{inputBits}";

                AttachInternalGlues();
                Tiles = new List<Tile> {first, second, third, fourth};
            }


            public string Result { get; }


            public Tile First { get; }


            public Tile Last { get; }


            private static string Name(string input, bool carry, int index) => $"GuessOne={input} {carry} {index} id: {Guid.NewGuid()}";


            public void Bind(IHaveFirst zero)
            {
                First.AttachAbove(zero.First);
            }


            private void AttachInternalGlues()
            {
                first.AttachNorth(second);
                second.AttachNorth(third);
                third.AttachNorth(fourth);
            }

        }

        private class GuessZero : IHaveFirst, IHaveLast
        {

            private readonly Tile fifth;

            private readonly Tile first;

            private readonly Tile fourth;

            private readonly Tile second;

            private readonly Tile third;

            public readonly List<Tile> Tiles;


            public GuessZero(string inputBits, bool carry, int index)
            {
                first  = new Tile(Name(inputBits, carry, index));
                second = new Tile(Name(inputBits, carry, index));
                third  = new Tile(Name(inputBits, carry, index));
                fourth = new Tile(Name(inputBits, carry, index));
                fifth  = new Tile(Name(inputBits, carry, index));

                First  = first;
                Last   = fifth;
                Result = $"0{inputBits}";
                AttachInternalGlues();

                Tiles = new List<Tile> {first, second, third, fourth, fifth};
            }


            public string Result { get; }


            public Tile First { get; }


            public Tile Last { get; }


            private static string Name(string input, bool carry, int index) => $"GuessZero={input} {carry} {index} id: {Guid.NewGuid()}";


            private void AttachInternalGlues()
            {
                first.AttachNorth(second);
                second.AttachNorth(third);
                third.AttachNorth(fourth);
                fourth.AttachBelow(fifth);
            }

        }

    }

}
