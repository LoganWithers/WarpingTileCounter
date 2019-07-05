namespace WarpingCounter.Gadgets.Warping.FirstWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    public class FirstWarpDigit2Case2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public FirstWarpDigit2Case2(Glue input, Glue outputNorth, Glue outputWest)
        {
            Tiles = Create();
            Tiles.PrependNamesWith(nameof(FirstWarpDigit2Case2));

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = outputNorth;
            Output.West  = outputWest;
        }

        public List<Tile> Create() => new List<Tile> { new Tile(Guid.NewGuid().ToString()) };
    }

}