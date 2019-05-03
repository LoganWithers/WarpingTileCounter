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

        private readonly int bitsPerDigit;

        private readonly bool carry;

        private readonly int rectangleWidth;

        public readonly List<Tile> Tiles;


        public ReturnDigit3ReadNextRow(bool carry, int bitsPerDigit, int numberOfRegions)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;
            rectangleWidth    = numberOfRegions * 6 - 1;

            Tiles = InitializeTiles();
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
        private List<Tile> InitializeTiles()
        {
            var build = new GadgetBuilder().Start();

            build.South(12);

            build.West();

            build.South(3)
                 .Down();

            build.South(14);

            build.SouthLine(bitsPerDigit, carry);

            build.South(11);

            build.South()
                 .Up()
                 .West()
                 .South()
                 .South()
                 .East();

            build.South(16);

            build.SouthLine(bitsPerDigit, carry);

            build.South()
                 .South()
                 .West()
                 .Down()
                 .South()
                 .South()
                 .Up();

            build.East(rectangleWidth);

            build.North(3)
                 .Down();

            return build.Tiles()
                        .ToList();
        }

    }

}
