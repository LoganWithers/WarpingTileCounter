namespace WarpingCounter.Seed
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Enums;
    using Common.Models;


    public class InitialDigitWriter : IHaveFirst, IHaveLast
    {

        public Tile First { get; }


        public Tile Last { get; }


        public readonly List<Tile> Tiles;

        private readonly WriteDirection direction;

        private readonly string bits;


        public InitialDigitWriter(string bits, WriteDirection direction = WriteDirection.SouthToNorth)
        {
            this.bits = bits;
            this.direction = direction;

            Tiles = InitTiles();

            First = Tiles.First();
            Last = Tiles.Last();
        }


        private List<Tile> InitTiles()
        {
            var b = new GadgetBuilder().Start();


            if (direction == WriteDirection.NorthToSouth)
            {
                foreach (var bit in bits)
                {
                    switch (bit)
                    {
                        case '0':

                            b.South()
                             .South()
                             .East()
                             .South()
                             .West()
                             .South();

                            break;
                        case '1':

                            b.South()
                             .South()
                             .Up()
                             .East()
                             .South()
                             .West()
                             .Down()
                             .South();

                            break;
                        default:

                            throw new ArgumentOutOfRangeException(nameof(bit));
                    }
                }
            }
            else
            {
                foreach (var bit in bits)
                {
                    switch (bit)
                    {
                        case '0':
                            b.North()
                             .North()
                             .East()
                             .North()
                             .West()
                             .North();
                            break;

                        case '1':
                            b.North()
                             .North()
                             .Up()
                             .East()
                             .North()
                             .West()
                             .Down()
                             .North();
                            break;

                        default:

                            throw new ArgumentOutOfRangeException(nameof(bit));
                    }
                }
            }

            var tiles = b.Tiles()
                         .ToList();

            tiles.PrependNamesWith($"Initial Digit: {bits}");

            return tiles.Skip(1)
                        .ToList();
        }

    }
}
