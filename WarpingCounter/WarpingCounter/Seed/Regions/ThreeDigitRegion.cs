namespace WarpingCounter.Seed.Regions
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Builders;
    using Common.Enums;
    using Common.Models;

    using Gadgets;

    public class ThreeDigitRegion
    {

        private readonly int bitsPerDigit;

        private readonly string digit1;

        private readonly string digit2;

        private readonly string digit3;

        private readonly bool leastSignificantRegion;

        private readonly int regionIndex;

        public readonly List<Tile> Tiles;


        public ThreeDigitRegion(int                                           bitsPerDigit,
                                (string digit3, string digit2, string digit1) encodings,
                                int                                           regionIndex,
                                bool                                          leastSignificantRegion)
        {
            this.bitsPerDigit           = bitsPerDigit;
            this.regionIndex            = regionIndex;
            (digit3, digit2, digit1)    = encodings;
            this.leastSignificantRegion = leastSignificantRegion;
            Tiles                       = InitializeTiles();
        }


        private Tile Last { get; set; }


        private Tile GetFirstTile()
        {
            if (regionIndex == 0)
            {
                return new Tile("seed");
            }

            var first = new Tile($"Region: {regionIndex} First BridgeConnect") {
                West = new Glue($"Region {regionIndex - 1}")
            };

            return first;
        }


        private List<Tile> InitializeTiles()
        {
            var firstTile = GetFirstTile();
            var build     = new GadgetBuilder().StartWith(firstTile);

            void North() => build.NorthLine(bitsPerDigit);

            build.NorthLine(bitsPerDigit);
            build.North(16);

            build.North()
                 .West()
                 .North()
                 .North()
                 .East()
                 .Down();

            build.North(11);

            North();

            build.North(14);

            build.North()
                 .Up()
                 .North()
                 .North()
                 .North()
                 .East();

            build.North(12);

            North();

            build.North(8);

            build.East()
                 .South()
                 .Down()
                 .North()
                 .North()
                 .North()
                 .West()
                 .North()
                 .East()
                 .North()
                 .West()
                 .North()
                 .North()
                 .North()
                 .Up()
                 .East()
                 .East();

            build.South(9);

            build.South()
                 .Down();

            build.South(4);

            var digit3 = new InitialDigitWriter(this.digit3, WriteDirection.NorthToSouth);

            List<Tile> tiles = build.Tiles()
                                    .ToList();

            tiles.PrependNamesWith($"Region: {regionIndex} msr");

            tiles.Last()
                 .AttachSouth(digit3.First);

            tiles.AddRange(digit3.Tiles);

            var b2 = new GadgetBuilder();
            b2.Start();
            b2.South(8);
            b2.West();

            b2.South(9)
              .Up();

            b2.South(4);

            b2.West()
              .Down();

            b2.South(8);
            b2.SouthLine(bitsPerDigit);

            b2.South(14)
              .Up()
              .East()
              .East()
              .Down();

            b2.South(10);

            b2.South()
              .West();

            b2.South(5);
            b2.SouthLine(bitsPerDigit);

            b2.South()
              .West()
              .West();

            b2.NorthLine(bitsPerDigit);
            b2.North(17);

            b2.North()
              .Up()
              .East();

            b2.North(12);
            b2.NorthLine(bitsPerDigit);

            b2.North(8)
              .East()
              .South()
              .Down()
              .North()
              .North()
              .North()
              .West()
              .North()
              .East()
              .North()
              .West()
              .North()
              .North()
              .North()
              .Up()
              .East()
              .East();

            b2.South(10)
              .Down();

            b2.South(4);

            List<Tile> secondTiles = b2.Tiles()
                                       .ToList();

            digit3.Last.AttachSouth(secondTiles.First());
            secondTiles.PrependNamesWith($"Region: {regionIndex} D3 to D2");
            tiles.AddRange(secondTiles);

            var digit2 = new InitialDigitWriter(this.digit2, WriteDirection.NorthToSouth);
            digit2.First.AttachNorth(tiles.Last());

            tiles.AddRange(digit2.Tiles);

            List<Tile> digit2ToDigit1 = BuildFromDigit2ToDigit1();

            digit2ToDigit1.First()
                          .AttachNorth(digit2.Last);

            tiles.AddRange(digit2ToDigit1);

            var digit1 = new InitialDigitWriter(this.digit1, WriteDirection.NorthToSouth);
            digit1.First.AttachNorth(tiles.Last());
            tiles.AddRange(digit1.Tiles);

            if (leastSignificantRegion)
            {
                AddCounterStartTiles(tiles.Last(), tiles);
            } else
            {
                AddRegionBridge(tiles.Last(), tiles);
            }

            return tiles;
        }


        private void AddCounterStartTiles(Tile lastAdded, List<Tile> tiles)
        {
            var regionEnd = new Tile(Guid.NewGuid()
                                         .ToString());

            lastAdded.AttachSouth(regionEnd);

            var readerStart = new Tile(Guid.NewGuid().ToString()) {
                North = GlueFactory.DigitReader(string.Empty, true, 1)
            };

            regionEnd.AttachEast(readerStart);
            tiles.AddRange(new[] {regionEnd, readerStart});
            Last = readerStart;
        }


        private void AddRegionBridge(Tile lastAdded, List<Tile> tiles)
        {
            var builder = new GadgetBuilder().Start();

            builder.South()
                   .Up()
                   .East()
                   .East();

            List<Tile> bridgeTiles = builder.Tiles()
                                            .ToList();

            bridgeTiles.RemoveAt(0);
            var firstBridgeTiles = bridgeTiles.First();

            lastAdded.AttachSouth(firstBridgeTiles);
            tiles.AddRange(bridgeTiles);

            Last      = bridgeTiles.Last();
            Last.East = new Glue($"Region {regionIndex}");
        }


        private List<Tile> BuildFromDigit2ToDigit1()
        {
            var first   = new Tile($"Region: {regionIndex}: {Guid.NewGuid().ToString()}");
            var builder = new GadgetBuilder().StartWith(first);

            builder.South(8)
                   .West();

            builder.South(9)
                   .Up();

            builder.South(4)
                   .West()
                   .Down();

            builder.South(8);
            builder.SouthLine(bitsPerDigit);

            var last = builder.Tiles()
                              .Last();

            var line = new SouthToNorthLine(bitsPerDigit);
            line.First.AttachBelow(last);

            var b2 = new GadgetBuilder().Start();

            var firstBack = b2.Tiles()
                              .First();

            firstBack.Prepend($"Region: {regionIndex} B2 FirstBack");
            line.Last.AttachNorth(firstBack);

            b2.North(7)
              .East()
              .South()
              .Down()
              .North()
              .North()
              .North()
              .West()
              .North()
              .East()
              .North()
              .West()
              .North()
              .North()
              .North()
              .Up()
              .East()
              .East();

            b2.South(10)
              .Down();

            b2.South(4);

            List<Tile> results = builder.Tiles()
                                        .ToList();

            results.AddRange(line.Tiles);
            results.AddRange(b2.Tiles());

            return results;
        }

    }

}
