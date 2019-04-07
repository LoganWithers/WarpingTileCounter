namespace WarpingCounter.Gadgets.Warping
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PreFirstWarp : IHaveFirst, IHaveLast
    {

        private readonly string bits;

        private readonly int digitsInMSR;

        public readonly List<Tile> Tiles;


        public PreFirstWarp(string bits, int index, bool carry, int digitsInMSR)
        {
            this.digitsInMSR = digitsInMSR;
            this.bits        = bits;

            Tiles = InitializeTiles();

            if (Tiles.None())
            {
                return;
            }

            Tiles.PrependNamesWith($"{nameof(PreFirstWarp)} {bits} {index} {carry}");

            First       = Tiles.First();
            First.South = GlueFactory.PreFirstWarp(bits, carry, index);

            Last       = Tiles.Last();
            Last.North = GlueFactory.FirstWarp(bits, index, carry);

        }


        public Tile First { get; }


        public Tile Last { get; }


        private List<Tile> InitializeTiles()
        {
            switch (digitsInMSR)
            {
                case 3:
                    return CreateForDigitsCase3();

                case 2:
                {
                    // Not in the MSR
                    if (bits.EndsWith("00"))
                    {
                        return CreateForDigitsCase3();
                    }

                    // Digit 1 in the MSR
                    if (bits.EndsWith("01"))
                    {
                        return CreateDigit1Case2();
                    }

                    // Digit 2 (MSD) in the MSR
                    if (bits.EndsWith("11"))
                    {
                        return CreateDigit2Case2();
                    }

                    throw new ArgumentOutOfRangeException(bits);
                }

                case 1:

                    return bits.EndsWith("11") ? CreateDigit1Case1() // Digit 1 in the MSR
                    : CreateForDigitsCase3();                        // Not in the MSR

                default:

                    throw new ArgumentOutOfRangeException(nameof(digitsInMSR));
            }
        }


        private List<Tile> CreateForDigitsCase3()
        {
            var builder = new GadgetBuilder();

            return builder.StartWith(new Tile())
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .West()
                          .North()
                          .North()
                          .North()
                          .Up()
                          .North()
                          .West()
                          .North()
                          .Down()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .North()
                          .End()
                          .Tiles()
                          .ToList();
        }


        private static List<Tile> CreateDigit1Case1()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(17);
            builder.West();
            builder.North(12);

            return builder.Tiles()
                          .ToList();
        }


        private static List<Tile> CreateDigit1Case2()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(9)
                   .Up()
                   .West()
                   .West()
                   .Down();

            return builder.Tiles()
                          .ToList();
        }


        private static List<Tile> CreateDigit2Case2()
        {
            var tile = new Tile(Guid.NewGuid()
                                    .ToString());

            // Only one tile is used.
            return new List<Tile> {tile};
        }

    }

}
