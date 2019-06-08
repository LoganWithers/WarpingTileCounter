namespace WarpingCounter.Gadgets.Warping
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarp : IHaveInput, IHaveOutput
    {

        private readonly string bits;

        private readonly bool carry;

        private readonly List<string> colors = new List<string> {"", "yellow", "purple", "green"};

        private readonly int digitsInMSR;

        private readonly int index;

        public readonly List<Tile> Tiles;


        public PostWarp(string bits, int index, bool carry, int digitsInMSR)
        {
            this.bits        = bits;
            this.index       = index;
            this.carry       = carry;
            this.digitsInMSR = digitsInMSR;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(PostWarp)} {bits} {index} {carry}");
            Input = Tiles.First();
            Output  = Tiles.Last();

            Output.North = GlueFactory.DigitWriter(bits, carry, index);

            foreach (var t in Tiles)
            {
                t.Color = colors[index];
            }
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            switch (digitsInMSR)
            {
                case 3:

                    return CreateDigitCase3();

                case 2:
                {
                    // Not in the MSR
                    if (bits.EndsWith("00"))
                    {
                        return CreateDigitCase3();
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
                                               : CreateDigitCase3(); // Not in the MSR

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

            builder.Tiles()
                   .First()
                   .West = GlueFactory.PostWarp(bits, index, carry);

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


        private List<Tile> CreateDigit1Case1()
        {
            var b = new GadgetBuilder().Start();

            b.East()
             .North(4)
             .Down()
             .North(16)
             .West()
             .North();

            b.Tiles()
             .First().Down = GlueFactory.PostWarp(bits, index, carry);

            return b.Tiles()
                    .ToList();
        }


        private List<Tile> CreateDigitCase3()
        {
            switch (index)
            {
                // First digit in a region has a slightly different path than D2/D3
                // (D1's PostWarp needs to go through the crossing region)  
                case 1:
                {
                    var b = new GadgetBuilder().Start();

                    b.East()
                     .North(4)
                     .Down()
                     .North(9)
                     .East()
                     .North(3)
                     .East()
                     .North(4)
                     .West()
                     .North();

                    var tiles = b.Tiles().ToList();

                    tiles.First().Down = GlueFactory.PostWarp(bits, index, carry);

                    return tiles;
                }

                default:
                {
                    var b = new GadgetBuilder();

                    b.Start()
                     .East()
                     .North(4)
                     .Down()
                     .North(9)
                     .East()
                     .North(8);


                    var tiles = b.Tiles().ToList();

                    tiles.First()
                         .Down = GlueFactory.PostWarp(bits, index, carry);

                    return tiles;
                }
            }
        }

    }

}
