namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using ReturnAndRead.NextRow;

    /// <summary>
    ///   A gadget that is used only for the third digit (MSD) in MSR when case 3.
    ///   this gadget is placed will alter the succeeding return and read gadget accordingly.
    ///   <br />
    ///   The first tile connects to a digit that ends with "11" and is
    ///   in a region with two other digits.
    ///   <br />
    ///   The last tile of this gadget connects to <see cref="ReturnDigit3ReadNextRow" />
    /// </summary>
    /// <seealso cref="IHaveOutput" />
    /// <seealso cref="IHaveInput" />
    public class DigitTopDigit3Case3 : IHaveOutput, IHaveInput
    {

        private readonly int L;

        public readonly List<Tile> Tiles;


        public DigitTopDigit3Case3(bool carry, int bitsPerDigit)
        {
            L = bitsPerDigit;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(DigitTopDigit3Case3)} {carry}");

            Input       = Tiles.First();
            Input.South = GlueFactory.DigitTopDigit3Case3(carry);

            Output       = Tiles.Last();
            Output.South = GlueFactory.ReturnDigit3ReadNextRow(carry);
        }


        public Tile Input { get; }


        public Tile Output { get; }


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

            return build.SouthLine(L).Tiles().ToList();
        }

    }

}
