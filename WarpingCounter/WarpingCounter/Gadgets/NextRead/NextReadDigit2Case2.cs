namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit2Case2 : IHaveInput, IHaveOutput
    {
        private readonly int L;

        public Tile Input { get; }

        public Tile Output { get; }

        public List<Tile> Tiles { get; }

        public NextReadDigit2Case2(string name, int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input        = Tiles.First();
            Input.North  = input;

            Output       = Tiles.Last();
            Output.East  = output;
        }

        /// <summary>
        /// 37 + 4L tiles
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(10)
                   .West()
                   .South(4)
                   .East()
                   .South(15)
                   .South(4 * L, "blue")
                   .South()
                   .West()
                   .South(3);

            return builder.Tiles().ToList();
        }
    }


}
