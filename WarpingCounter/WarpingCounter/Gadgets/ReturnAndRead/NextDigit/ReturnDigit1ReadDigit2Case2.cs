namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnDigit1ReadDigit2Case2 : IHaveFirst, IHaveLast
    {

        private const int NextDigitRead = 2;

        private readonly int bitsPerDigit;

        public readonly List<Tile> Tiles;


        public ReturnDigit1ReadDigit2Case2(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;

            Tiles = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit1ReadDigit2Case2)} {carry}");

            First       = Tiles.First();
            First.North = GlueFactory.ReturnDigit1ReadDigit2Case2(carry);

            Last       = Tiles.Last();
            Last.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile First { get; }


        public Tile Last { get; }


        private List<Tile> InitializeTiles()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(29);
            builder.SouthLine(bitsPerDigit);
            builder.South(30);
            builder.SouthLine(bitsPerDigit);

            builder.South()
                   .Up()
                   .East()
                   .East()
                   .East()
                   .Down();

            return builder.Tiles()
                          .ToList();
        }

    }

}
