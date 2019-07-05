namespace WarpingCounter.Gadgets.Warping.WarpBridge
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class WarpBridgeDigit3 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public WarpBridgeDigit3(Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.PrependNamesWith(nameof(WarpBridgeDigit3));

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