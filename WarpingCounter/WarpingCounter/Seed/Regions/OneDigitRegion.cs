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

        private readonly int bitsPerDigit;

        private readonly string digit1;

        public readonly List<Tile> Tiles;


        public OneDigitRegion(int bitsPerDigit, string digit1)
        {
            this.bitsPerDigit = bitsPerDigit;
            this.digit1       = digit1;
            Tiles             = Init();
            Last              = Tiles.Last();
            Last.East         = new Glue("Region 0");
        }


        public Tile Last { get; }


        private List<Tile> Init()
        {
            var tiles = new List<Tile>();
            var line  = new SouthToNorthLine(bitsPerDigit);

            var seed = new Tile("seed") {
                North = new Glue(line.First.North.Label)
            };

            tiles.Add(seed);
            tiles.AddRange(line.Tiles);

            var b = new GadgetBuilder().StartWith(line.Last);

            b.North(16)
             .West();

            b.North(4)
             .East();

            b.North(10);

            tiles.AddRange(b.Tiles());

            var line2 = new SouthToNorthLine(bitsPerDigit);

            tiles.Last()
                 .AttachNorth(line2.First);

            tiles.AddRange(line2.Tiles);

            b = new GadgetBuilder().StartWith(line2.Last);
            b.North(30);
            tiles.AddRange(b.Tiles());

            var line3 = new SouthToNorthLine(bitsPerDigit);

            tiles.Last()
                 .AttachNorth(line3.First);

            tiles.AddRange(line3.Tiles);

            b = new GadgetBuilder().StartWith(line3.Last);

            b.North(15)
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
             .South()
             .Up()
             .North()
             .West();

            b.South(7);

            tiles.AddRange(b.Tiles());

            var line4 = new NorthToSouthLine(bitsPerDigit);

            tiles.Last()
                 .AttachSouth(line4.First);

            tiles.AddRange(line4.Tiles);

            b = new GadgetBuilder().StartWith(line4.Last);
            b.South(30);
            tiles.AddRange(b.Tiles());

            var line5 = new NorthToSouthLine(bitsPerDigit);

            tiles.Last()
                 .AttachSouth(line5.First);

            tiles.AddRange(line5.Tiles);

            b = new GadgetBuilder().StartWith(line5.Last);

            b.South(14)
             .Down();

            b.South(16);

            tiles.AddRange(b.Tiles());

            var digit = new InitialDigitWriter(digit1, WriteDirection.NorthToSouth);

            tiles.Last()
                 .AttachSouth(digit.First);

            tiles.AddRange(digit.Tiles);

            // Bridge to non-msr regions
            b = new GadgetBuilder().Start();

            b.South()
             .Up()
             .East()
             .East();

            List<Tile> bridgeTiles = b.Tiles()
                                      .ToList();

            bridgeTiles.RemoveAt(0);
            var firstBridgeTiles = bridgeTiles.First();

            tiles.Last()
                 .AttachSouth(firstBridgeTiles);

            tiles.AddRange(bridgeTiles);

            return tiles;
        }

    }

}
