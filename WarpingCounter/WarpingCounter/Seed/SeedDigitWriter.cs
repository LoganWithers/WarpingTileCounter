namespace WarpingCounter.Seed
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Enums;
    using Common.Models;

    /// <summary>
    /// Takes as input a digit value encoded in binary,
    /// and generates the tiles required to assemble that digit from
    /// south to north.
    ///
    /// <remarks>
    /// Used only in the construction of the seed.
    /// </remarks>
    /// </summary>
    public class CreateialDigitWriter : IHaveInput, IHaveOutput
    {

        private readonly string bits;

        private readonly WriteDirection direction;


        public readonly List<Tile> Tiles;


        public CreateialDigitWriter(string bits, WriteDirection direction = WriteDirection.SouthToNorth)
        {
            this.bits      = bits;
            this.direction = direction;

            Tiles = CreateTiles();

            Input = Tiles.First();
            Output  = Tiles.Last();
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> CreateTiles()
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
            } else
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

            List<Tile> tiles = b.Tiles()
                                .ToList();

            tiles.PrependNamesWith($"Createial Digit: {bits}");

            return tiles.Skip(1)
                        .ToList();
        }

    }

}
