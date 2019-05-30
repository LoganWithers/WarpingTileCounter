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


        public IFromNorthGadgetBuilder North(string name = null) => new FromNorthGadgetBuilder(tiles, this, name);


        public IFromSouthGadgetBuilder South(string name = null) => new FromSouthGadgetBuilder(tiles, this, name);


        public IFromEastGadgetBuilder East(string name = null) => new FromEastGadgetBuilder(tiles, this, name);


        public IFromWestGadgetBuilder West(string name = null) => new FromWestGadgetBuilder(tiles, this, name);


        public IFromNorthGadgetBuilder North(int n)
        {
            IFromNorthGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = North();
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


        public IFromEastGadgetBuilder East(int n)
        {
            IFromEastGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = East();
            }

            return lastBuilder;
        }


        public IFromWestGadgetBuilder West(int n)
        {
            IFromWestGadgetBuilder lastBuilder = null;

            for (var i = 0; i < n; i++)
            {
                lastBuilder = West();
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
