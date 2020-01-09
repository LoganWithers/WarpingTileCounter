namespace WarpingCounter.Gadgets.DigitTop
{
    using Common;
    using Common.Builders;
    using Common.Models;

    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   The first tile of this gadget is connected to the last tile of a digit when it ends with "00".
    ///   The last tile is connected to a return and read gadget.
    /// </summary>
    public class DigitTop : IHaveInput, IHaveOutput
    {
        public readonly List<Tile> Tiles;

        private readonly int L;

        public DigitTop(string name, int L, Glue input, Glue output)
        {
            this.L = L;
            Tiles  = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.South = output;
        }

        public Tile Input { get; }

        public Tile Output { get; }

        /// <summary>
        /// 40 + 4L tiles
        /// </summary>
        private List<Tile> Create()
        {
            var build = new GadgetBuilder().Start();

            build.North(4)
                 .Up()
                 .North(10)
                 .West(2)
                 .Down()
                 .South(3)
                 .East()
                 .South()
                 .West()
                 .South()
                 .East()
                 .South(3)
                 .Up()
                 .North()
                 .West()
                 .South(7)
                 .South(4 * L, "blue");

            return build.Tiles()
                        .ToList();
        }
    }
}
