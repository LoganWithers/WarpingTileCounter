namespace WarpingCounter.Seed.Regions
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Enums;
    using Common.Models;

    public class TwoDigitRegion : IHaveLast
    {

        private readonly int bitsPerDigit;

        private readonly string digit1;

        private readonly string digit2;

        public readonly List<Tile> Tiles;


        public TwoDigitRegion(int bitsPerDigit, (string digit2, string digit1) digits)
        {
            this.bitsPerDigit = bitsPerDigit;
            (digit2, digit1)  = digits;

            Tiles     = Init();
            Last      = Tiles.Last();
            Last.East = new Glue("Region 0");
        }


        public Tile Last { get; }


        private List<Tile> Init()
        {
            var seed = new Tile("seed");

            var tiles   = new List<Tile> {seed};
            var builder = new GadgetBuilder().StartWith(seed);

            builder.South()
                   .Up()
                   .East()
                   .South()
                   .South()
                   .Down();

            builder.South(4);
            tiles.AddRange(builder.Tiles());

            var line1 = new NorthToSouthLine(bitsPerDigit);

            tiles.Last()
                 .AttachSouth(line1.First);

            tiles.AddRange(line1.Tiles);

            builder = new GadgetBuilder().StartWith(line1.Last);
            builder.South(30);
            tiles.AddRange(builder.Tiles());

            var digit2 = new InitialDigitWriter(this.digit2, WriteDirection.NorthToSouth);

            tiles.Last()
                 .AttachSouth(digit2.First);

            tiles.AddRange(digit2.Tiles);

            builder = new GadgetBuilder().StartWith(digit2.Last);

            builder.South(25)
                   .West();

            builder.South(5);
            builder.SouthLine(bitsPerDigit);

            builder.South()
                   .West();

            builder.NorthLine(bitsPerDigit);

            builder.North(9)
                   .East()
                   .South()
                   .South()
                   .Up()
                   .East()
                   .South()
                   .South()
                   .Down();

            builder.South(4);
            tiles.AddRange(builder.Tiles());

            var digit1 = new InitialDigitWriter(this.digit1, WriteDirection.NorthToSouth);

            tiles.Last()
                 .AttachSouth(digit1.First);

            tiles.AddRange(digit1.Tiles);

            builder = new GadgetBuilder().Start();

            builder.South()
                   .Up()
                   .East()
                   .East();

            List<Tile> bridgeTiles = builder.Tiles()
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
