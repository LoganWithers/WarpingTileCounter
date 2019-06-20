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

        private readonly int d;
        public Tile Input { get; }
        public Tile Output { get; }
        public CrossNextRow(int d, bool op)
        {
            this.d = d;
            Tiles = Create();

            Input      = Tiles.First();
            Input.West = GlueFactory.Create(Names.CrossNextRow, op);

            Output       = Tiles.Last();
            Output.North = GlueFactory.Create(Names.Read, 1, string.Empty, op);
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            var width = (6 * (int) Math.Floor((decimal) d / 3)) - 1;

            builder.East(width)
                   .North(3)
                   .Down();


            return builder.Tiles().ToList();
        }
        public IEnumerable<Tile> Tiles { get;  }
    }
}
