namespace WarpingCounter.Gadgets.Warping.SecondWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    public class SecondWarpDigit2 : IHaveInput, IHaveOutput
    {

        public Tile Input { get; }


        public Tile Output { get; }


        public readonly List<Tile> Tiles;


        public SecondWarpDigit2(string name, Glue input, Glue outputNorth, Glue outputUp)
        {
            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = outputNorth;
            Output.Up    = outputUp;
        }


        public List<Tile> Create() => new List<Tile> {
        new Tile(Guid.NewGuid()
                     .ToString())
        };

    }

}
