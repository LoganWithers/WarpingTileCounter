namespace WarpingCounter.Gadgets.NextRead
{
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit2 : IHaveInput, IHaveOutput
    {
        private readonly int L;

        public Tile Input { get; }

        public Tile Output { get; }

        public List<Tile> Tiles { get; }

        public NextReadDigit2(string name, int L, Glue input, Glue output)
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
        /// 41 + 4L tiles
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(14)
                   .East()
                   .Up()
                   .East(3)
                   .South(12)
                   .West(2)
                   .South(3)
                   .South(4 * L, "blue")
                   .South()
                   .East(2)
                   .Down();

            return builder.Tiles().ToList();
        }
    }


}
