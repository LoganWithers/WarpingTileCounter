namespace WarpingCounter.Common.Builders
{
    using System;
    using System.Collections.Generic;

    using Interfaces;

    using Models;

    public class FromSouthGadgetBuilder : IFromSouthGadgetBuilder
    {
        private readonly GadgetBuilder original;

        private readonly LinkedList<Tile> tiles;

        public FromSouthGadgetBuilder(LinkedList<Tile> tiles, GadgetBuilder original, string color = "white", string name = null)
        {
            this.original = original;
            var previous = tiles.Last.Value;

            var next = new Tile(name ??
                                Guid.NewGuid()
                                    .ToString()) {Color = color};

            previous.AttachSouth(next);
            tiles.AddLast(next);

            this.tiles = tiles;
        }

        public IEnumerable<Tile> Tiles => tiles;

        public IFromUpGadgetBuilder Up(string color = "white", string name = null) => new FromUpGadgetBuilder(tiles, original, color, name);

        public IFromDownGadgetBuilder Down(string color = "white", string name = null) => new FromDownGadgetBuilder(tiles, original, color, name);

        public IFromEastGadgetBuilder East(string color = "white", string name = null) => new FromEastGadgetBuilder(tiles, original, color, name);

        public IFromWestGadgetBuilder West(string color = "white", string name = null) => new FromWestGadgetBuilder(tiles, original, color, name);

        public IFromSouthGadgetBuilder South(string color = "white", string name = null) => new FromSouthGadgetBuilder(tiles, original, color, name);

        public IFromEastGadgetBuilder East(int n, string color = "white")
        {
            IFromEastGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = East(color);
            }

            return lastBuilder;
        }

        public IFromWestGadgetBuilder West(int n, string color = "white")
        {
            IFromWestGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = West(color);
            }

            return lastBuilder;
        }

        public IFromSouthGadgetBuilder South(int n, string color = "white")
        {
            IFromSouthGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = South(color);
            }

            return lastBuilder;
        }

        public IGadgetBuilder End() => original;
    }
}
