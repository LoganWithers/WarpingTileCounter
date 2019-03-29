namespace WarpingCounter.Gadgets.Warping
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class WarpBridge : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;
        public Tile First { get; }
        public Tile Last { get; }

        private readonly int digitsInMSR;
        private readonly string bits;

        private readonly int index;
        private readonly bool carry;


        public WarpBridge(string bits, int index, bool carry, int digitsInMSR)
        {
            this.bits        = bits;
            this.index = index;
            this.carry = carry;
            this.digitsInMSR = digitsInMSR;

            Tiles = Init();

            if (Tiles.Any())
            {
                First = Tiles.First();
                Last  = Tiles.Last();

                Tiles.PrependNamesWith($"{nameof(WarpBridge)} bits={bits} index={index} carry={carry}");
                
                Last.North = GlueFactory.SecondWarp(bits, index, carry);
            }

        }

        private List<Tile> Init()
        {
            switch (digitsInMSR)
            {
                case 3:
                    return CreateForThreeDigits();

                case 2:
                {
                    // Not in the MSR
                    if (bits.EndsWith("00"))
                    {
                        return CreateForThreeDigits();
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
                    return bits.EndsWith("11") ? CreateDigit1Case1()        // Digit 1 in the MSR
                                               : CreateForThreeDigits(); // Not in the MSR

                default:
                    throw new ArgumentOutOfRangeException(nameof(digitsInMSR));

            }
        }

        // Case 1 Only uses pre-warp, warp, and post warp, thus no tiles should be added
        private List<Tile> CreateDigit1Case1() => new List<Tile>();


        // Digit 1 goes from first warp to post warp, skipping 
        // the warp bridge
        private List<Tile> CreateDigit1Case2() => new List<Tile>();


        private List<Tile> CreateForThreeDigits()
        {
            var builder = new GadgetBuilder();

            builder.StartWith(new Tile());

            builder.North(11)
                   .Up()
                   .West()
                   .West()
                   .Down();

            builder.North(13);

            var tiles = builder.Tiles()
                               .ToList();

            tiles.First().West = GlueFactory.WarpBridge(bits, index, carry);

            return builder.Tiles()
                          .ToList();
        }


        private List<Tile> CreateDigit2Case2()
        {
            var builder = new GadgetBuilder().Start();
            builder.West();

            var tiles = builder.Tiles()
                               .ToList();

            tiles.First().East = GlueFactory.WarpBridge(bits, index, carry);
            return builder.Tiles().ToList();
        }
    }
}
