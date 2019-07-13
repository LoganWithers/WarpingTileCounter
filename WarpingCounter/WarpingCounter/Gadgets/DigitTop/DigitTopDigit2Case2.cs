namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using ReturnPath;

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
    ///   The last tile of this gadget connects to <see cref="ReturnPathDigit2Case2" />
    /// </summary>
    /// <seealso cref="IHaveOutput" />
    /// <seealso cref="IHaveInput" />
    public class DigitTopDigit2Case2 : IHaveOutput, IHaveInput
    {

        private readonly int L;

        public readonly List<Tile> Tiles;


        public DigitTopDigit2Case2(string name, int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input        = Tiles.First();
            Input.South  = input;

            Output       = Tiles.Last();
            Output.South = output;
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(29)
                   .North(4 * L, "blue")
                   .North(5)
                   .Up()
                   .North(2)
                   .West()
                   .Down()
                   .North()
                   .Up()
                   .East(3)
                   .South(7)
                   .South(4 * L, "blue");

            return builder.Tiles().ToList();
        }

    }

}
