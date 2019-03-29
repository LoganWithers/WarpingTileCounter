namespace WarpingCounter.Gadgets.Warping
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarp : IHaveFirst, IHaveLast
    {
        public Tile First { get; }
        public Tile Last { get; }

        private readonly List<string> colors = new List<string>{"", "yellow", "purple", "green"};
        public readonly List<Tile> Tiles;

        private readonly int digitsInMSR;
        private readonly string bits;
        private readonly int index;
        private readonly bool carry;

        public PostWarp(string bits, int index, bool carry, int digitsInMSR)
        {
            this.bits = bits;
            this.index = index;
            this.carry = carry;
            this.digitsInMSR = digitsInMSR;

            Tiles = Init();
            Tiles.PrependNamesWith($"{nameof(PostWarp)} bits={bits} index={index} carry={carry}");
            First = Tiles.First();
            Last  = Tiles.Last();


            Last.North = GlueFactory.WriteDigit(bits, index, carry);

            foreach (var t in Tiles)
            {
                t.Color = colors[index];
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
                    return bits.EndsWith("11") ? CreateMSDCase1() // Digit 1 in the MSR
                                               : CreateForThreeDigits();                     // Not in the MSR

                default:
                    throw new ArgumentOutOfRangeException(nameof(digitsInMSR));

            }
        }


        private List<Tile> CreateDigit1Case2()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(3)
                   .Up()
                   .North()
                   .North()
                   .Down()
                   .East()
                   .North()
                   .West();

            builder.North(18);

            builder.Tiles().First().West = GlueFactory.PostWarp(bits, index, carry);

            return builder.Tiles()
                          .ToList();
        }


        private List<Tile> CreateDigit2Case2()
        {
            var builder = new GadgetBuilder().Start();
            // TODO: 24 is not right, just guess until I can test 
            builder.North(24);

            builder.Tiles().First().West = GlueFactory.PostWarp(bits, index, carry);

            return builder.Tiles()
                          .ToList();
        }


        private List<Tile> CreateMSDCase1()
        {
            var builder = new GadgetBuilder().Start();

            builder.East()
                   .North()
                   .North()
                   .North()
                   .North()
                   .Down();

            builder.North(16)
                   .West()
                   .North();
            builder.Tiles().First().Down = GlueFactory.PostWarp(bits, index, carry);
            return builder.Tiles()
                          .ToList();
        }


        private List<Tile> CreateForThreeDigits()
        {
            var builder = new GadgetBuilder();

            switch (index)
            {   
                // First digit in a region has a slightly different path than D2/D3
                // (D1's PostWarp needs to go through the crossing region)  
                case 1:
                {
                    var tiles = builder.StartWith(new Tile())
                                       .East()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .Down()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .East()
                                       .North()
                                       .North()
                                       .North()
                                       .East()
                                       .North()
                                       .North()
                                       .North()
                                       .North()
                                       .West()
                                       .North()
                                       .End()
                                       .Tiles()
                                       .ToList();

                    tiles.First().Down = GlueFactory.PostWarp(bits, index, carry);
                    return tiles;
                }

                default:

                {
                    builder.StartWith(new Tile())
                           .East()
                           .North()
                           .North()
                           .North()
                           .North()
                           .Down();

                    builder.North(9);
                    builder.East();

                    var tiles = builder.North(8)
                                       .End()
                                       .Tiles()
                                       .ToList();

                    tiles.First().Down = GlueFactory.PostWarp(bits, index, carry);

                        return tiles;
                }
            }
        }
    }
}
