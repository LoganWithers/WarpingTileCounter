namespace WarpingCounter.Gadgets.Warping.PostWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarpDigit2Case2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PostWarpDigit2Case2(Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(PostWarpDigit2Case2)} {Guid.NewGuid()}");

            Input      = Tiles.First();
            Input.West = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(22);

            return builder.Tiles().ToList();
        }
    }
}
