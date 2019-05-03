namespace WarpingCounter.Common.Builders
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    public class NorthToSouthLine : IHaveInput, IHaveOutput
    {

        public readonly List<Tile> Tiles;


        public NorthToSouthLine(int count, bool? carry = null)
        {
            var builder = new GadgetBuilder();

            for (var i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    builder.StartWith(new Tile(Guid.NewGuid()
                                                   .ToString()))
                           .South()
                           .South()
                           .South();
                } else
                {
                    builder.South()
                           .South()
                           .South()
                           .South();
                }
            }

            Tiles = builder.Tiles()
                           .ToList();

            Tiles.PrependNamesWith($"{nameof(NorthToSouthLine)} carry={carry}");
            Input = Tiles.First();
            Output  = Tiles.Last();

            foreach (var tile in Tiles)
            {
                tile.Color = "blue";
            }
        }


        public Tile Input { get; }


        public Tile Output { get; }

    }

}
