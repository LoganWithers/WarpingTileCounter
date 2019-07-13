namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using ReturnPath;


    /// <summary>
    ///   A gadget that is used only for the first digit in a MSR when it's case 2.
    ///   <br />
    ///   This digit top is special in that its tiles are mostly assembled in the z=0 plane,
    ///   contrary to a standard digit top.
    /// 
    ///   <br />
    ///   The last tile of this gadget connects to <see cref="ReturnPathDigit1Case1" />
    /// </summary>
    /// <seealso cref="IHaveOutput" />
    /// <seealso cref="IHaveInput" />
    public class DigitTopDigit1Case1 : IHaveInput, IHaveOutput
    {

        private readonly int L;

        public readonly List<Tile> Tiles;
        public DigitTopDigit1Case1(string name, int L, Glue input, Glue output)
        {
            this.L = L;
            Tiles         = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.South = output;
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(16)
                   .Up()
                   .North(13)
                   .North(4 * L, "blue")
                   .North(30)
                   .North(4 * L, "blue")
                   .North(8)
                   .East()
                   .South()
                   .Down()
                   .North(3)
                   .West()
                   .North()
                   .East()
                   .North()
                   .West()
                   .North(3)
                   .Up()
                   .East(2)
                   .South(14)
                   .South(4 * L, "blue");


            return builder.Tiles().ToList();
        }

    }

}
