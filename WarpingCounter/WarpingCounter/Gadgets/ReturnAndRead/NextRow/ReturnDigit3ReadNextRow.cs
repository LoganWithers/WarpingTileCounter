namespace WarpingCounter.Gadgets.ReturnAndRead.NextRow
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    ///   Gadget that is used in case 3, after writing digit 3 (MSD). Crosses and
    ///   attaches a blank reader to begin reading the next row.
    /// </summary>
    /// <seealso cref="IHaveInput" />
    /// <seealso cref="IHaveOutput" />
    public class ReturnDigit3ReadNextRow : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 1;

        private readonly int tilesPerDigit;

        private readonly int rectangleWidth;

        public readonly List<Tile> Tiles;


        public ReturnDigit3ReadNextRow(bool carry, int bits, int numberOfRegions)
        {
            tilesPerDigit  = bits * 4;
            rectangleWidth = numberOfRegions * 6 - 1;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit3ReadNextRow)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit3ReadNextRow(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        private List<Tile> Create()
        {
            var build = new GadgetBuilder().Start();

            build.South(12)
                 .West()
                 .South(3)
                 .Down()
                 .South(14)
                 .South(tilesPerDigit)
                 .South(12)
                 .Up()
                 .West()
                 .South(2)
                 .East()
                 .South(16)
                 .South(tilesPerDigit)
                 .South(2)
                 .West()
                 .Down()
                 .South(2)
                 .Up()
                 .East(rectangleWidth)
                 .North(3)
                 .Down();

            return build.Tiles()
                        .ToList();
        }

    }

}
