namespace WarpingCounter.Gadgets.ReturnAndRead.NextRow
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    /// Gadget that is used only in case 1, after writing digit 1 (MSD). Crosses and
    /// attaches a blank reader to begin reading the next row.
    /// </summary>
    /// <seealso cref="WarpingCounter.Common.IHaveFirst" />
    /// <seealso cref="WarpingCounter.Common.IHaveLast" />
    public class ReturnDigit1ReadNextRow : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;

        public Tile First { get; }
        public Tile Last  { get; }
        
        private const int NextDigitRead = 1;

        private readonly int bitsPerDigit;
        private readonly int rectangleWidth;


        public ReturnDigit1ReadNextRow(bool carry, int bitsPerDigit, int totalOtherRegions)
        {
            this.bitsPerDigit = bitsPerDigit;
            rectangleWidth = 6 * (totalOtherRegions - 1);

            Tiles = Init();
            Tiles.PrependNamesWith($"ReturnDigit1ReadNextRow {carry}");

            First = Tiles.First();
            First.South = GlueFactory.ReturnD1ReadD1(carry);

            Last = Tiles.Last();
            Last.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        private List<Tile> Init()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(16).Up();
            builder.North(13);
            builder.NorthLine(bitsPerDigit);
            builder.North(30);
            builder.NorthLine(bitsPerDigit);

            builder.North(8)
                   .East()
                   .South()
                   .Down()
                   .North()
                   .North()
                   .North()
                   .West()
                   .North()
                   .East()
                   .North()
                   .West()
                   .North()
                   .North()
                   .North()
                   .Up()
                   .East()
                   .East();

            builder.South(14);
            builder.SouthLine(bitsPerDigit);
            builder.South(30);
            builder.SouthLine(bitsPerDigit);
            builder.South(11).West();
            builder.South(4);
            builder.East();
            builder.South(15);
            builder.SouthLine(bitsPerDigit);

            builder.South()
                   .West();

            builder.South(3);
            builder.East(rectangleWidth);
            builder.North(3).Down();

            return builder.Tiles()
                          .ToList();
        }
    }
}
