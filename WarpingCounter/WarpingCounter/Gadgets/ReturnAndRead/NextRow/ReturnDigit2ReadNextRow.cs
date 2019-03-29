namespace WarpingCounter.Gadgets.ReturnAndRead.NextRow
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    /// Gadget that is used only in case 2, after writing digit 2 (MSD). Crosses and
    /// attaches a blank reader to begin reading the next row.
    /// </summary>
    /// <seealso cref="WarpingCounter.Common.IHaveFirst" />
    /// <seealso cref="WarpingCounter.Common.IHaveLast" />
    public class ReturnDigit2ReadNextRow : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;
        public Tile First { get; }
        public Tile Last  { get; }


        private const int NextDigitRead = 1;
        
        private readonly int rectangleWidth;
        
        private readonly int bitsPerDigit;

        public ReturnDigit2ReadNextRow(bool carry, int bitsPerDigit, int numberOfRegions)
        {
            this.bitsPerDigit = bitsPerDigit;
            rectangleWidth    = (numberOfRegions - 1) * 6;

            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit2ReadNextRow)} carry={carry}");

            First       = Tiles.First();
            First.North = GlueFactory.ReturnD2CrossReadD1(carry);

            Last        = Tiles.Last();
            Last.North  = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }

        private List<Tile> InitializeTiles()
        {
            var build = new GadgetBuilder().Start();

            build.South(29);
            build.SouthLine(bitsPerDigit);
            build.South(11);
            build.West();

            build.South(4)
                 .East();

            build.South(15);

            build.SouthLine(bitsPerDigit);

            build.South(1).West();
            build.South(3);


            build.East(rectangleWidth);

            build.North(3)
                 .Down();


            return build.Tiles().ToList();
        }
    }
}
