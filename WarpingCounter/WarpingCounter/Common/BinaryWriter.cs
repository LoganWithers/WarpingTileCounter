namespace WarpingCounter.Common
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Builders;
    using Builders.Interfaces;

    using Gadgets;

    using Models;

    public class BinaryWriter : IHaveInput, IHaveOutput
    {

        private readonly string originalBits;

        public readonly List<Tile> Tiles;


        public BinaryWriter(string originalBits, bool carry, int index, int digitsInMSR)
        {
            this.originalBits = originalBits;
            Tiles             = CreateTiles();
            Tiles.PrependNamesWith($"Write {originalBits} {carry} {index}");
            Input       = Tiles.First();
            Input.South = GlueFactory.DigitWriter(originalBits, carry, index);

            Output       = Tiles.Last();
            Output.North = DetermineGadgetToAttachTo(digitsInMSR, originalBits, carry, index);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private Glue DetermineGadgetToAttachTo(int digitsInMSR, string bits, bool carry, int index)
        {
            switch (digitsInMSR)
            {
                case 1 when bits.EndsWith("11"): return GlueFactory.ReturnDigit1ReadNextRow(carry);

                case 2 when bits.EndsWith("01"): return GlueFactory.DigitTopDigit1Case2(carry);

                case 2 when bits.EndsWith("11"): return GlueFactory.DigitTopDigit2Case2(carry);

                case 3 when bits.EndsWith("11"): return GlueFactory.DigitTopDigit3Case3(carry);

                default:                         return GlueFactory.DigitTop(carry, index);
            }
        }


        private List<Tile> CreateTiles()
        {
            var encoder = new BinaryToTileEncoder();

            var bits = StringUtils.Reverse(originalBits);

            foreach (var bit in bits)
            {
                switch (bit)
                {
                    case '1':
                        encoder.AddOne();
                        break;

                    case '0':
                        encoder.AddZero();
                        break;

                    default:

                        throw new Exception($"Invalid bit found in: {originalBits}");
                }
            }

            return encoder.End();
        }


        private class BinaryToTileEncoder
        {

            private readonly IGadgetBuilder builder;


            public BinaryToTileEncoder()
            {
                builder = new GadgetBuilder().Start();
            }


            public void AddOne()
            {
                builder.North()
                       .North()
                       .Up()
                       .East()
                       .North()
                       .West()
                       .Down()
                       .North();
            }


            public void AddZero()
            {
                builder.North()
                       .North()
                       .East()
                       .North()
                       .West()
                       .North();
            }


            public List<Tile> End() => builder.Tiles()
                                              .Skip(1)
                                              .ToList();

        }

    }

}
