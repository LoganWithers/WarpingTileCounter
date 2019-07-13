namespace WarpingCounter.Gadgets.Warping.PostWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarpDigit1Case2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PostWarpDigit1Case2(string name, Glue input, Glue output)
        {
            Tiles = Create();            Tiles.RenameWithIndex(name);

            Input      = Tiles.First();
            Input.West = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
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
