namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class DigitTopDigit1Case2 : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;

        public Tile Last { get; }
        public Tile First { get; }

        private const int Index = 1;
        private readonly int bitsPerDigit;

        public DigitTopDigit1Case2(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(DigitTopDigit1Case2)} carry={carry} {Index}");

            First = Tiles.First();
            Last  = Tiles.Last();

            First.South = GlueFactory.DigitTopDigit1Case2(carry);
            Last.South  = GlueFactory.ReturnD1ReadD2Case2(carry);
        }


        private List<Tile> InitializeTiles()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(4)
                   .Up()
                   .North()
                   .North()
                   .West()
                   .Down()
                   .North()
                   .West();

            builder.South(7);
            builder.SouthLine(bitsPerDigit);

            return builder.Tiles()
                          .ToList();
        }
    }
}
