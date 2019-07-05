﻿namespace WarpingCounter.Gadgets
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

        public CrossNextRow(int d, Glue input, Glue output)
        {
            this.d = d;
            Tiles = Create();
            Tiles.PrependNamesWith(nameof(CrossNextRow));

            Input      = Tiles.First();
            Input.West = input;

            Output       = Tiles.Last();
            Output.North = output;
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
