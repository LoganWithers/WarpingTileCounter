namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;

    using Interfaces;

    using Models;

    public class FromEastGadgetBuilder : IFromEastGadgetBuilder
    {

        private readonly GadgetBuilder original;

        private readonly LinkedList<Tile> tiles;


        public FromEastGadgetBuilder(LinkedList<Tile> tiles, GadgetBuilder original, string name = null)
        {
            this.original = original;
            var previous = tiles.Last.Value;

            var next = new Tile(name ??
                                Guid.NewGuid()
                                    .ToString());

            previous.AttachEast(next);
            tiles.AddLast(next);

            this.tiles = tiles;
        }


        public IEnumerable<Tile> Tiles => tiles;


        public IFromNorthGadgetBuilder North(string name = null) => new FromNorthGadgetBuilder(tiles, original, name);


        public IFromSouthGadgetBuilder South(string name = null) => new FromSouthGadgetBuilder(tiles, original, name);


        public IFromEastGadgetBuilder East(string name = null) => new FromEastGadgetBuilder(tiles, original, name);


        public IFromUpGadgetBuilder Up(string name = null) => new FromUpGadgetBuilder(tiles, original, name);


        public IFromDownGadgetBuilder Down(string name = null) => new FromDownGadgetBuilder(tiles, original, name);

        public IFromEastGadgetBuilder East(int n)
        {
            IFromEastGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = East();
            }

            return lastBuilder;
        }


        public IFromSouthGadgetBuilder South(int n)
        {
            IFromSouthGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = South();
            }

            return lastBuilder;
        }


        public IFromNorthGadgetBuilder North(int n)
        {
            IFromNorthGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = North();
            }

            return lastBuilder;
        }

        public IGadgetBuilder End() => original;

    }

}
