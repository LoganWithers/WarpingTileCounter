namespace WarpingCounter.Gadgets.ReturnAndRead.NextRow
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    ///   Gadget that is used only in case 1, after writing digit 1 (MSD). Crosses and
    ///   attaches a blank reader to begin reading the value of the counter.
    /// </summary>
    /// <seealso cref="IHaveInput" />
    /// <seealso cref="IHaveOutput" />
    public class ReturnDigit1ReadNextRow : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 1;

        /// <summary>
        ///    Ceil( log M ) + 2
        /// </summary>
        private readonly int L;

        private readonly int rectangleWidth;

        public readonly List<Tile> Tiles;


        public ReturnDigit1ReadNextRow(bool carry, int bits, int numberOfRegions)
        {
            L  = bits * 4;
            rectangleWidth = 6 * (numberOfRegions - 1);

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit1ReadNextRow)} {carry}");

            Input       = Tiles.First();
            Input.South = GlueFactory.ReturnDigit1ReadNextRow(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(30)
                   .South(L)
                   .South(11)
                   .West()
                   .South(4)
                   .East()
                   .South(15)
                   .South(L)
                   .South()
                   .West()
                   .South(3)
                   .East(rectangleWidth)
                   .North(3)
                   .Down();

            return builder.Tiles()
                          .ToList();
        }

    }

}
