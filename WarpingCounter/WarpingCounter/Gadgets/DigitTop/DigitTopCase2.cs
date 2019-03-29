namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class MsdDigitTopCase2 : IHaveLast, IHaveFirst
    {
        public readonly List<Tile> Tiles;

        public Tile First { get; }
        public Tile Last  { get; }

        private readonly int bitsPerDigit;


        public MsdDigitTopCase2(bool carry, int bitsPerDigit)
        {

            this.bitsPerDigit = bitsPerDigit;

            Tiles = InitializeTiles();
            Tiles.PrependNamesWith("DigitTopDigit2Case2 ");

            First = Tiles.First();
            First.South = GlueFactory.MsdTopCase2(carry);

            Last  = Tiles.Last();
            Last.South = GlueFactory.ReturnD2CrossReadD1(carry);
        }


        private List<Tile> InitializeTiles()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(29);

            builder.NorthLine(bitsPerDigit);

            builder.North(5)
                   .Up();

            builder.North(2)
                   .West()
                   .Down()
                   .North()
                   .Up()
                   .East()
                   .East()
                   .East();


            builder.South(7);
            builder.SouthLine(bitsPerDigit);

            return builder.Tiles()
                          .ToList();
        }
    }
}
