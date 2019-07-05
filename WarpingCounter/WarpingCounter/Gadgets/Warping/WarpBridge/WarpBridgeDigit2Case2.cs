namespace WarpingCounter.Gadgets.Warping.WarpBridge
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class WarpBridgeDigit2Case2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public WarpBridgeDigit2Case2(Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.PrependNamesWith(nameof(WarpBridgeDigit2Case2));

            Input       = Tiles.First();
            Input.East  = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();


            builder.West()
                   .North(13);


            return builder.Tiles().ToList();
        }
    }

}
