﻿namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;

    using Interfaces;

    using Models;

    public class FromWestGadgetBuilder : IFromWestGadgetBuilder
    {

        private readonly LinkedList<Tile> tiles;
        public IEnumerable<Tile> Tiles => tiles;


        private readonly GadgetBuilder original;

        public FromWestGadgetBuilder(LinkedList<Tile> tiles, GadgetBuilder original, string name = null)
        {
            this.original = original;

            var previous = tiles.Last.Value;
            var next     = new Tile(name ?? Guid.NewGuid().ToString());

            previous.AttachWest(next);
            tiles.AddLast(next);
            this.tiles = tiles;
        }
        
        public IFromUpGadgetBuilder       Up(string name = null) => new FromUpGadgetBuilder(tiles, original, name);
        public IFromDownGadgetBuilder   Down(string name = null) => new FromDownGadgetBuilder(tiles, original, name);
        public IFromWestGadgetBuilder   West(string name = null) => new FromWestGadgetBuilder(tiles, original, name);
        public IFromNorthGadgetBuilder North(string name = null) => new FromNorthGadgetBuilder(tiles, original, name);
        public IFromSouthGadgetBuilder South(string name = null) => new FromSouthGadgetBuilder(tiles, original, name);


        public IGadgetBuilder End() => original;

    }

}