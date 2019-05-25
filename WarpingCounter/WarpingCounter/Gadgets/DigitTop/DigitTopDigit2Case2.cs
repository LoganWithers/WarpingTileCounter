namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using ReturnAndRead.NextRow;

    /// <summary>
    ///   A gadget that is used only for the second digit (MSD) in a MSR when it's case 2.
    ///   <br />
    ///   This digit top is special in that it builds to the right (east) instead of going west.
    ///   It then assembles south in the z=1 plane, above the tiles that assembled as part of
    ///   its neighbor digit region to the east.
    /// 
    ///   The first tile connects to a digit that ends with "11" and is
    ///   in a region with one other digit.
    ///   <br />
    ///   The last tile of this gadget connects to <see cref="ReturnDigit2ReadNextRow" />
    /// </summary>
    /// <seealso cref="IHaveOutput" />
    /// <seealso cref="IHaveInput" />
    public class DigitTopDigit2Case2 : IHaveOutput, IHaveInput
    {

        private readonly int bitsPerDigit;

        public readonly List<Tile> Tiles;


        public DigitTopDigit2Case2(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;

            Tiles = Create();
            Tiles.PrependNamesWith($"DigitTopDigit2Case2 {carry}");

            Input       = Tiles.First();
            Input.South = GlueFactory.DigitTopDigit2Case2(carry);

            Output       = Tiles.Last();
            Output.South = GlueFactory.ReturnDigit2ReadNextRow(carry);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
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

            return builder.Tiles().ToList();
        }

    }

}
