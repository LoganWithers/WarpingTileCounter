namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;

    using Interfaces;

    using Models;

    public class FromDownGadgetBuilder : IFromDownGadgetBuilder
    {

        private readonly GadgetBuilder original;

        private readonly LinkedList<Tile> tiles;


        public FromDownGadgetBuilder(LinkedList<Tile> tiles, GadgetBuilder original, string name = null)
        {
            this.original = original;
            var previous = tiles.Last.Value;

            var next = new Tile(name ??
                                Guid.NewGuid()
                                    .ToString());

            previous.AttachBelow(next);
            tiles.AddLast(next);

            this.tiles = tiles;
        }


        public IEnumerable<Tile> Tiles => tiles;


        public IFromEastGadgetBuilder East(string name = null) => new FromEastGadgetBuilder(tiles, original, name);


        public IFromWestGadgetBuilder West(string name = null) => new FromWestGadgetBuilder(tiles, original, name);


        public IFromSouthGadgetBuilder South(string name = null) => new FromSouthGadgetBuilder(tiles, original, name);


        public IFromNorthGadgetBuilder North(string name = null) => new FromNorthGadgetBuilder(tiles, original, name);


        public IGadgetBuilder End() => original;

    }

}
