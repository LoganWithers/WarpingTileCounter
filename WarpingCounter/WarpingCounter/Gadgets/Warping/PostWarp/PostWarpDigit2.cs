namespace WarpingCounter.Gadgets.Warping.PostWarp
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarpDigit2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PostWarpDigit2(string name, Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input      = Tiles.First();
            Input.Down = input;

            Output       = Tiles.Last();
            Output.North = output;
        }

        /// <summary>
        /// 25 tiles
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.East()
                   .North(4)
                   .Down()
                   .North(9)
                   .East()
                   .North(8);

            return builder.Tiles().ToList();
        }

    }

}
