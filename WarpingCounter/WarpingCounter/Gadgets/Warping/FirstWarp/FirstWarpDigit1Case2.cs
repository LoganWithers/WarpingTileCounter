﻿namespace WarpingCounter.Gadgets.Warping.FirstWarp
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    public class FirstWarpDigit1Case2 : IHaveInput, IHaveOutput
    {

        public Tile Input { get; }


        public Tile Output { get; }


        public readonly List<Tile> Tiles;


        public FirstWarpDigit1Case2(string name, Glue input, Glue outputNorth, Glue outputEast)
        {
            Tiles = Create();
            Tiles.RenameWithIndex(name);
            
            Input       = Tiles.First();
            Input.South = input;
            Input.Color = "yellow";
            Output       = Tiles.Last();
            Output.North = outputNorth;
            Output.East  = outputEast;
        }


        public List<Tile> Create() => new List<Tile> {
                new Tile(Guid.NewGuid().ToString())
        };

    }

}
