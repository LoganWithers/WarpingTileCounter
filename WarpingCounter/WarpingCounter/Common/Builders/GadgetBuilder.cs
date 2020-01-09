namespace WarpingCounter.Common.Builders
{
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    using Models;

    public class GadgetBuilder : IGadgetBuilder
    {
        private readonly LinkedList<Tile> tiles = new LinkedList<Tile>();

        private Tile Last => tiles.Last.Value;

        public IGadgetBuilder StartWith(Tile tile)
        {
            tiles.AddFirst(tile);

            return this;
        }

        public IGadgetBuilder Start()
        {
            tiles.AddFirst(new Tile());

            return this;
        }

        public IFromNorthGadgetBuilder North(string color = "white", string name = null) => new FromNorthGadgetBuilder(tiles, this, color, name);

        public IFromSouthGadgetBuilder South(string color = "white", string name = null) => new FromSouthGadgetBuilder(tiles, this, color, name);

        public IFromEastGadgetBuilder East(string color = "white", string name = null) => new FromEastGadgetBuilder(tiles, this, color, name);

        public IFromWestGadgetBuilder West(string color = "white", string name = null) => new FromWestGadgetBuilder(tiles, this, color, name);

        public IFromNorthGadgetBuilder North(int n, string color = "white")
        {
            IFromNorthGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = North(color);
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

        public IFromEastGadgetBuilder East(int n, string color = "white")
        {
            IFromEastGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = East();
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

        public IGadgetBuilder NorthLine(int numberOfBits)
        {
            var line = new SouthToNorthLine(numberOfBits);

            if (tiles.Any())
            {
                line.Input.AttachSouth(tiles.Last.Value);
            }

            tiles.AppendRange(line.Tiles);

            return this;
        }

        public IGadgetBuilder SouthLine(int numberOfBits)
        {
            var line = new NorthToSouthLine(numberOfBits);

            if (tiles.Any())
            {
                line.Input.AttachNorth(tiles.Last.Value);
            }

            tiles.AppendRange(line.Tiles);

            return this;
        }

        public IEnumerable<Tile> Tiles() => tiles;
    }
}
