namespace WarpingCounter.Seed
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
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
    public class DigitWriter : IHaveInput, IHaveOutput
    {

        private readonly string bits;
        
        public readonly List<Tile> Tiles;


        public DigitWriter(string bits, Glue input, Glue output)
        {
            this.bits      = bits;

            Tiles = CreateTiles();
            Tiles.PrependNamesWith($"Digit {Guid.NewGuid()}");

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> CreateTiles()
        {
            var builder = new GadgetBuilder().Start();

            foreach (var bit in bits)
            {
                switch (bit)
                {
                    case '0':
                        builder.North(2)
                               .East()
                               .North()
                               .West()
                               .North();

                        break;

                    case '1':

                        builder.North(2)
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
            
            // Skip 1 since the first tile is not apart of the encoded digits. 
            return builder.Tiles().Skip(1).ToList();
        }

    }

}
