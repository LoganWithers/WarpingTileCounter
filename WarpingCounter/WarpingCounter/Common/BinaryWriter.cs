namespace WarpingCounter.Common
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Builders;
    using Builders.Interfaces;

    using Gadgets;

    using Models;

    public class BinaryWriter : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;

        public Tile First { get; }
        public Tile Last  { get; }


        private readonly string originalBits;

        public BinaryWriter(string originalBits, bool carry, int index, int digitsInMSR)
        {
            this.originalBits = originalBits;
            Tiles = InitTiles();
            Tiles.PrependNamesWith($"WRITE bits={originalBits} {carry} {index}");
            First       = Tiles.First();
            First.South = GlueFactory.DigitWriter(originalBits, carry, index);

            Last        = Tiles.Last();
            Last.North  = DetermineGadgetToAttachTo(digitsInMSR, originalBits, carry, index);
        }


        private Glue DetermineGadgetToAttachTo(int digitsInMSR, string bits, bool carry, int index)
        {

            if (digitsInMSR == 1 && bits.EndsWith("11"))
            {
                return GlueFactory.ReturnDigit1ReadNextRow(carry);
            }

            if (digitsInMSR == 3 && bits.EndsWith("11"))
            {
                Debug.Assert(digitsInMSR == 3, "digitsInMSR == 3");
                return GlueFactory.DigitTopDigit3Case3(carry);
            }

            if (digitsInMSR == 2 && bits.EndsWith("11"))
            {
                return GlueFactory.DigitTopDigit2Case2(carry);
            }

            if (digitsInMSR == 2 && bits.EndsWith("01"))
            {
                return GlueFactory.DigitTopDigit1Case2(carry);
            }

            return GlueFactory.DigitTopDefault(carry, index);
        }

        private List<Tile> InitTiles()
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

            public BinaryToTileEncoder()
            {
                builder = new GadgetBuilder().Start();
            }

            private readonly IGadgetBuilder builder;


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
