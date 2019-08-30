namespace WarpingCounter.Gadgets.Warping.PostWarp
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarpDigit1Case1 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PostWarpDigit1Case1(string name, Glue input, Glue output)
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
                   .North(16)
                   .West()
                   .North();

            return builder.Tiles().ToList();
        }

    }

}
