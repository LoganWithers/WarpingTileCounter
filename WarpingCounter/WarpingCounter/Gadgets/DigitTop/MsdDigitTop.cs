namespace WarpingCounter.Gadgets.DigitTop
{
    using System.Collections.Generic;
    using System.Linq;


    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    /// A gadget that is used after writing the third digit (MSD) in a region, thus it
    /// will alter the succeeding return and read gadget accordingly. 
    ///
    /// The first tile connects to a digit that ends with "11" and is
    /// in a region with two other digits.
    /// 
    /// The last tile of this gadget connects to <see cref=""/>
    /// </summary>
    /// <seealso cref="IHaveLast" />
    /// <seealso cref="IHaveFirst" />
    public class MsdDigitTop : IHaveLast, IHaveFirst
    {

        public readonly List<Tile> Tiles;

        public Tile Last  { get; }
        public Tile First { get; }

        private readonly bool carry;
        private readonly int bitsPerDigit;


        public MsdDigitTop(bool carry, int bitsPerDigit)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(MsdDigitTop)} carry={carry} index={3}");

            First       = Tiles.First();
            First.South = GlueFactory.MsdTopCase3(carry);

            Last        = Tiles.Last();
            Last.South  = GlueFactory.ReturnD3CrossReadD1(carry);
        }


        private List<Tile> InitializeTiles()
        {
            var build = new GadgetBuilder().Start();

            build.North(4)
                 .Up();
            build.North(10)
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

            build.South(7);

            return build.SouthLine(bitsPerDigit, carry)
                        .Tiles()
                        .ToList();

        }



    }
}
