namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnDigit2ReadDigit3 : IHaveFirst, IHaveLast
    {

        private const int NextDigitRead = 3;

        private readonly int bitsPerDigit;

        private readonly bool carry;

        public readonly List<Tile> Tiles;


        public ReturnDigit2ReadDigit3(bool carry, int bitsPerDigit)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;

            Tiles = InitTiles();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit2ReadDigit3)} carry={carry}");

            First       = Tiles.First();
            First.North = GlueFactory.ReturnDigit2ReadDigit3(carry);

            Last       = Tiles.Last();
            Last.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile First { get; }


        public Tile Last { get; }


        private List<Tile> InitTiles()
        {
            var build = new GadgetBuilder().Start();

            build.South(12);

            build.West()
                 .Down();

            build.South(17);

            build.SouthLine(bitsPerDigit, carry);

            build.South(15)
                 .East()
                 .Up()
                 .East()
                 .East()
                 .East();

            build.South(11);

            build.South()
                 .West()
                 .West()
                 .South()
                 .South()
                 .South();

            build.SouthLine(bitsPerDigit, carry);

            build.South()
                 .East()
                 .East()
                 .Down();

            return build.Tiles()
                        .ToList();
        }

    }

}
