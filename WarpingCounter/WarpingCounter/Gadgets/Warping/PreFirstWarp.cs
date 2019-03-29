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

        public readonly List<Tile> Tiles;

        private readonly int digitsInMSR;

        private readonly string bits;

        public PreFirstWarp(string bits, int index, bool carry, int digitsInMSR)
        {
            this.digitsInMSR = digitsInMSR;
            this.bits = bits;

            Tiles = InitTiles();

            if (Tiles.Any())
            {
                First      = Tiles.First();
                Last       = Tiles.Last();
                First.South = GlueFactory.PreFirstWarp(bits, carry, index);
                Last.North = GlueFactory.FirstWarp(bits, index, carry);
                Tiles.PrependNamesWith($"{nameof(PreFirstWarp)} bits={bits} index={index} carry={carry}");
            }
        }


        private List<Tile> InitTiles()
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
                        return CreateNonMSDCase2();
                    }

                    // Digit 2 (MSD) in the MSR
                    if (bits.EndsWith("11"))
                    {
                        return CreateMSDCase2();
                    }

                    throw new ArgumentOutOfRangeException(bits);
                }

                case 1:
                    return bits.EndsWith("11") ? CreateMSDCase1()    // Digit 1 in the MSR
                                               : CreateForThreeDigits(); // Not in the MSR
            
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitsInMSR));

            }
        }

        private List<Tile> CreateForThreeDigits()
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


        private List<Tile> CreateMSDCase1()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(17);
            builder.West();

            return builder.Tiles()
                          .ToList();
        }


        private List<Tile> CreateNonMSDCase2()
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


        private List<Tile> CreateMSDCase2()
        {
            var tile = new Tile(Guid.NewGuid()
                                    .ToString());

            // No tiles are used in this case
            return new List<Tile> { tile };

        }


        public Tile First { get; }
        public Tile Last { get; }

    }

}