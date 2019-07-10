namespace WarpingCounter.Gadgets.Warping.PostWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PostWarpDigit3 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PostWarpDigit3(Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(PostWarpDigit3)} {Guid.NewGuid()}");

            Input      = Tiles.First();
            Input.Down = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
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