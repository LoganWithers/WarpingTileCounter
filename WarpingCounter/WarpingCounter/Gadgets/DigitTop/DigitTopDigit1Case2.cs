namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    ///   Gadget
    /// </summary>
    /// <seealso cref="IHaveFirst" />
    /// <seealso cref="IHaveLast" />
    public class DigitTopDigit1Case2 : IHaveFirst, IHaveLast
    {

        private const int Index = 1;

        private readonly int bitsPerDigit;

        public readonly List<Tile> Tiles;


        public DigitTopDigit1Case2(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(DigitTopDigit1Case2)} {carry} {Index}");

            First       = Tiles.First();
            First.South = GlueFactory.DigitTopDigit1Case2(carry);

            Last       = Tiles.Last();
            Last.South = GlueFactory.ReturnDigit1ReadDigit2Case2(carry);
        }


        public Tile First { get; }


        public Tile Last { get; }


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
