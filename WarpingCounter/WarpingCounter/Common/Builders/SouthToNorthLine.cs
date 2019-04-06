namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    public class SouthToNorthLine : IHaveFirst, IHaveLast
    {

        public readonly List<Tile> Tiles;


        public SouthToNorthLine(int bitsLong)
        {
            var builder = new GadgetBuilder();

            for (var i = 0; i < bitsLong; i++)
            {
                if (i == 0)
                {
                    builder.StartWith(new Tile(Guid.NewGuid()
                                                   .ToString()))
                           .North()
                           .North()
                           .North();
                } else
                {
                    builder.North()
                           .North()
                           .North()
                           .North();
                }
            }

            Tiles = builder.Tiles()
                           .ToList();

            Tiles.PrependNamesWith($"{nameof(SouthToNorthLine)} %seed%");
            First = Tiles.First();
            Last  = Tiles.Last();

            foreach (var tile in Tiles)
            {
                tile.Color = "blue";
            }
        }


        public Tile First { get; }


        public Tile Last { get; }

    }

}
