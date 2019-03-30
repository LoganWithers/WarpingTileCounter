namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class DigitTopDigit2Case2 : IHaveLast, IHaveFirst
    {
        public readonly List<Tile> Tiles;

        public Tile First { get; }
        public Tile Last  { get; }

        private readonly int bitsPerDigit;


        public DigitTopDigit2Case2(bool carry, int bitsPerDigit)
        {

            this.bitsPerDigit = bitsPerDigit;

            Tiles = InitializeTiles();
            Tiles.PrependNamesWith($"DigitTopDigit2Case2 {carry}");

            First       = Tiles.First();
            First.South = GlueFactory.DigitTopDigit2Case2(carry);

            Last        =  Tiles.Last();
            Last.South  = GlueFactory.ReturnDigit2ReadNextRow(carry);
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
