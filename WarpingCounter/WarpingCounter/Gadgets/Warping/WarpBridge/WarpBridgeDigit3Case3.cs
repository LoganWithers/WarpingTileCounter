namespace WarpingCounter.Gadgets.Warping.WarpBridge
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using PreWarp;

    public class WarpBridgeDigit3Case3 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public WarpBridgeDigit3Case3(string name, Glue input, Glue output)
        {
            Tiles = Create();            Tiles.RenameWithIndex(name);

            Input      = Tiles.First();
            Input.West = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();


            builder.North(11)
                   .Up()
                   .West()
                   .West()
                   .Down()
                   .North(13);


            return builder.Tiles().ToList();
        }
    }

}