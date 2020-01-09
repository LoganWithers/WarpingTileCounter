namespace WarpingCounter.Gadgets.Warping.WarpBridge
{
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class WarpBridgeDigit2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public WarpBridgeDigit2(string name, Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input      = Tiles.First();
            Input.West = input;

            Output       = Tiles.Last();
            Output.North = output;
        }

        /// <summary>
        /// 29 tiles
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(11)
                   .Up()
                   .West(2)
                   .Down()
                   .North(13);

            return builder.Tiles()
                          .ToList();
        }
    }
}
