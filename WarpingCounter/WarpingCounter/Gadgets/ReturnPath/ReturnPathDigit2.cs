﻿namespace WarpingCounter.Gadgets.ReturnPath
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnPathDigit2 : IHaveInput, IHaveOutput
    {

        private readonly int L;


        public Tile Input { get; }


        public Tile Output { get; }


        public IEnumerable<Tile> Tiles { get; }


        public ReturnPathDigit2(int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.PrependNamesWith(nameof(ReturnPathDigit2));

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.South = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(12)
                   .West()
                   .Down()
                   .South(17)
                   .South(4 * L, "blue");

            return builder.Tiles()
                          .ToList();
        }

    }

}
