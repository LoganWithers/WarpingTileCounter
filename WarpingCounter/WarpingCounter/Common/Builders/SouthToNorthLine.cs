namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    public class SouthToNorthLine : IHaveInput, IHaveOutput
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
                           .North(3);
                } else
                {
                    builder.North(4);
                }
            }

            Tiles = builder.Tiles()
                           .ToList();

            Tiles.PrependNamesWith($"{nameof(SouthToNorthLine)} %seed%");
            Input  = Tiles.First();
            Output = Tiles.Last();

            foreach (var tile in Tiles)
            {
                tile.Color = "blue";
            }
        }


        public Tile Input { get; }


        public Tile Output { get; }

    }

}
