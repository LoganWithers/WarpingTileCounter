namespace WarpingCounter.Seed.Regions
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Enums;
    using Common.Models;

    public class OneDigitRegion : IHaveLast
    {

        private readonly string digit1;

        private readonly int bitsPerDigit;

        public readonly List<Tile> Tiles;
        public Tile Last { get; }

        public OneDigitRegion(int bitsPerDigit, string digit1)
        {
            this.bitsPerDigit = bitsPerDigit;
            this.digit1 = digit1;
            Tiles = Init();
            Last = Tiles.Last();
            Last.East = new Glue("Region 0");
        }


        private List<Tile> Init()
        {
            var tiles = new List<Tile>();
            var line = new SouthToNorthLine(bitsPerDigit);

            var seed = new Tile("seed") {
                North = new Glue(line.First.North.Label)
            };

            tiles.Add(seed);
            tiles.AddRange(line.Tiles);

            var builder = new GadgetBuilder().StartWith(line.Last);
            builder.North(16).West();
            builder.North(4).East();
            builder.North(10);

            tiles.AddRange(builder.Tiles());
            var line2 = new SouthToNorthLine(bitsPerDigit);

            tiles.Last().AttachNorth(line2.First);
            tiles.AddRange(line2.Tiles);
            var builderB = new GadgetBuilder().StartWith(line2.Last);
            builderB.North(30);
            tiles.AddRange(builderB.Tiles());

            var line3 = new SouthToNorthLine(bitsPerDigit);
            tiles.Last().AttachNorth(line3.First);
            tiles.AddRange(line3.Tiles);

            var builder3 = new GadgetBuilder().StartWith(line3.Last);

            builder3.North(15)
                    .West()
                    .West()
                    .Down()
                    .South()
                    .South()
                    .South()
                    .East()
                    .South()
                    .West()
                    .South()
                    .East()
                    .South()
                    .South()
                    .South().Up().North().West();

            builder3.South(7);

            tiles.AddRange(builder3.Tiles());

            var line4 = new NorthToSouthLine(bitsPerDigit);
            tiles.Last().AttachSouth(line4.First);

            tiles.AddRange(line4.Tiles);

            var builder4 = new GadgetBuilder().StartWith(line4.Last);
            builder4.South(30);
            tiles.AddRange(builder4.Tiles());

            var line5 = new NorthToSouthLine(bitsPerDigit);
            tiles.Last().AttachSouth(line5.First);

            tiles.AddRange(line5.Tiles);

            var builder5 = new GadgetBuilder().StartWith(line5.Last);

            builder5.South(14)
                    .Down();

            builder5.South(16);

            tiles.AddRange(builder5.Tiles());

            var digit = new InitialDigitWriter(digit1, WriteDirection.NorthToSouth);

            tiles.Last()
                 .AttachSouth(digit.First);

            tiles.AddRange(digit.Tiles);

            var bridge = new GadgetBuilder().Start();

            bridge.South()
                  .Up()
                  .East()
                  .East();

            var bridgeTiles = bridge.Tiles().ToList();
            bridgeTiles.RemoveAt(0);
            var firstBridgeTiles = bridgeTiles.First();

            tiles.Last().AttachSouth(firstBridgeTiles);

            tiles.AddRange(bridgeTiles);


            return tiles;
        }
    }
}
