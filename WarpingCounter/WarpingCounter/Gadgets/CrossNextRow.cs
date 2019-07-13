namespace WarpingCounter.Gadgets
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class CrossNextRow : IHaveInput, IHaveOutput
    {

        private readonly int digits;

        public Tile Input { get; }

        public Tile Output { get; }

        public CrossNextRow(string name, int digits, Glue input, Glue output)
        {
            this.digits = digits;
            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input      = Tiles.First();
            Input.West = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();
            var difference = digits % 3 == 0 ? -1 : 0; 
            var generalRegions = (int) Math.Floor((decimal) digits / 3) + difference;

            builder.East(generalRegions * 6)
                   .North(3)
                   .Down();

            // Skip one for the extra tile at the start
            return builder.Tiles().Skip(1).ToList();
        }

        public IEnumerable<Tile> Tiles { get;  }
    }
}
