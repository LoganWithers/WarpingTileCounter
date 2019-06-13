namespace WarpingCounter.Gadgets.ReturnAndRead.NextRow
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    ///   Gadget that is used only in case 2, after writing digit 2 (MSD). Crosses and
    ///   attaches a blank reader to begin reading the next row.
    /// </summary>
    /// <seealso cref="IHaveInput" />
    /// <seealso cref="IHaveOutput" />
    public class ReturnDigit2ReadNextRow : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 1;

        private readonly int L;

        private readonly int rectangleWidth;

        public readonly List<Tile> Tiles;


        public ReturnDigit2ReadNextRow(bool carry, int bits, int numberOfRegions)
        {
            L  = bits * 4;
            rectangleWidth = (numberOfRegions - 1) * 6;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit2ReadNextRow)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit2ReadNextRow(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var build = new GadgetBuilder().Start();

            build.South(29)
                 .South(L)
                 .South(11)
                 .West()
                 .South(4)
                 .East()
                 .South(15)
                 .South(L)
                 .South(1)
                 .West()
                 .South(3)
                 .East(rectangleWidth)
                 .North(3)
                 .Down();

            return build.Tiles()
                        .ToList();
        }

    }

}
