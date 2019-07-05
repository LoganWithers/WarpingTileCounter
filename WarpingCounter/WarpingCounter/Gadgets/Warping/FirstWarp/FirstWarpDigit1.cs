namespace WarpingCounter.Gadgets.Warping.FirstWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    public class FirstWarpDigit1 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }
        
        public readonly List<Tile> Tiles;

        public FirstWarpDigit1(Glue input, Glue outputNorth, Glue outputEast)
        {
            Tiles = Create();
            Tiles.PrependNamesWith(nameof(FirstWarpDigit1));

            Input = Tiles.First();
            Input.South = input;

            Output = Tiles.Last();
            Output.North = outputNorth;
            Output.East  = outputEast;
        }

        public List<Tile> Create() => new List<Tile>{ new Tile(Guid.NewGuid().ToString()) };

    }

}
