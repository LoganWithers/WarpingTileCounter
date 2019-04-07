namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using ReturnAndRead.NextRow;

    /// <summary>
    ///   A gadget that is used only in case 3. After writing the third digit (MSD) in MSR,
    ///   this gadget is placed will alter the succeeding return and read gadget accordingly.
    ///   <br />
    ///   The first tile connects to a digit that ends with "11" and is
    ///   in a region with two other digits.
    ///   <br />
    ///   The last tile of this gadget connects to <see cref="ReturnDigit3ReadNextRow" />
    /// </summary>
    /// <seealso cref="IHaveLast" />
    /// <seealso cref="IHaveFirst" />
    public class DigitTopDigit3Case3 : IHaveLast, IHaveFirst
    {

        private readonly int bitsPerDigit;

        private readonly bool carry;

        public readonly List<Tile> Tiles;


        public DigitTopDigit3Case3(bool carry, int bitsPerDigit)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(DigitTopDigit3Case3)} {carry}");

            First       = Tiles.First();
            First.South = GlueFactory.DigitTopDigit3Case3(carry);

            Last       = Tiles.Last();
            Last.South = GlueFactory.ReturnDigit3ReadNextRow(carry);
        }


        public Tile First { get; }


        public Tile Last { get; }


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
