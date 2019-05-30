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

            guessZero.Output.North = zero;
            guessOne.Output.North  = one;
            guessOne.Input.South = input;

            Tiles = new List<Tile>();
            Tiles.AddRange(guessOne.Tiles);
            Tiles.AddRange(guessZero.Tiles);

            if (inputBits.Length + 1 == length && inputBits.All(bit => bit == '1'))
            {
                guessOne.Output.North.Bind  = 0;
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
            var indicatorBits = inputBits.GetLast(2);

            // The bits relevant to the value of the digit.
            var bits = inputBits.Substring(0, inputBits.Length - 2);

            // Convert these bits to its integer representation. 
            var value = Convert.ToInt32(bits, binary);

            bool CanAddOne(int n) => n + 1 <= baseM - 1;

            if (CanAddOne(value))
            {
                var incremented = Convert.ToString(value + 1, binary)
                                         .PadLeft(bits.Length, '0');

                // take the newly incremented value, and
                // set the carry signal to false since we incremented in this row. 
                return ($"{incremented}{indicatorBits}", false);
            }

            // Set all the bits to 0 since adding a value to the current value will 
            // result in some value that is not less than base M. Propagate the 
            // carry signal.
            var carryOut = string.Concat(Enumerable.Repeat("0", bits.Length));

            return ($"{carryOut}{indicatorBits}", true);
        }


        private class GuessOne : IHaveInput, IHaveOutput
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

                Input  = first;
                Output   = fourth;
                Result = $"1{inputBits}";

                AttachInternalGlues();
                Tiles = new List<Tile> {first, second, third, fourth};
            }


            public string Result { get; }


            public Tile Input { get; }


            public Tile Output { get; }


            private static string Name(string input, bool carry, int index) => $"GuessOne={input} {carry} {index} id: {Guid.NewGuid()}";


            public void Bind(IHaveInput zero)
            {
                Input.AttachAbove(zero.Input);
            }


            private void AttachInternalGlues()
            {
                first.AttachNorth(second);
                second.AttachNorth(third);
                third.AttachNorth(fourth);
            }

        }

        private class GuessZero : IHaveInput, IHaveOutput
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

                Input  = first;
                Output   = fifth;
                Result = $"0{inputBits}";
                AttachInternalGlues();

                Tiles = new List<Tile> {first, second, third, fourth, fifth};
            }


            public string Result { get; }


            public Tile Input { get; }


            public Tile Output { get; }


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
