namespace WarpingCounter.Gadgets.Warping.PreWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PreWarpDigit2Case2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PreWarpDigit2Case2(Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(PreWarpDigit2Case2)} {Guid.NewGuid()}");

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(29);

            return builder.Tiles().ToList();
        }
    }

}