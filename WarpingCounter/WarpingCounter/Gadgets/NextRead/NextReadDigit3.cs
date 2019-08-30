namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit3 : IHaveInput, IHaveOutput
    {
        private readonly int L;

        public Tile Input { get; }

        public Tile Output { get; }

        public List<Tile> Tiles { get; }

        public NextReadDigit3(string name, int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.North = output;
        }

        /// <summary>
        /// 94 + 12L tiles
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(2)
                   .West()
                   .Down()
                   .South(27)
                   .South(4 * L, "blue")
                   .South(30)
                   .South(4 * L, "blue")
                   .South(30)
                   .South(4 * L, "blue")
                   .South()
                   .West();

            return builder.Tiles().ToList();
        }
    }


}
