namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;

    using Interfaces;

    using Models;

    public class FromNorthGadgetBuilder : IFromNorthGadgetBuilder
    {

        private readonly GadgetBuilder original;

        private readonly LinkedList<Tile> tiles;


        public FromNorthGadgetBuilder(LinkedList<Tile> tiles, GadgetBuilder original, string name = null)
        {
            this.original = original;

            var previous = tiles.Last.Value;

            var next = new Tile(name ??
                                Guid.NewGuid()
                                    .ToString());

            previous.AttachNorth(next);
            tiles.AddLast(next);

            this.tiles = tiles;
        }


        public IEnumerable<Tile> Tiles => tiles;


        public IFromUpGadgetBuilder Up(string name) => new FromUpGadgetBuilder(tiles, original, name);


        public IFromDownGadgetBuilder Down(string name) => new FromDownGadgetBuilder(tiles, original, name);


        public IFromEastGadgetBuilder East(string name) => new FromEastGadgetBuilder(tiles, original, name);


        public IFromWestGadgetBuilder West(string name) => new FromWestGadgetBuilder(tiles, original, name);


        public IFromNorthGadgetBuilder North(string name) => new FromNorthGadgetBuilder(tiles, original, name);


        public IGadgetBuilder End() => original;

    }

}
