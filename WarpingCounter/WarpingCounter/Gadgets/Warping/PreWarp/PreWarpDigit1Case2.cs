namespace WarpingCounter.Gadgets.Warping.PreWarp
{
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PreWarpDigit1Case2 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;

        public PreWarpDigit1Case2(string name, Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = output;
        }

        /// <summary>
        /// 34 tiles
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(14)
                   .Up()
                   .West(2)
                   .Down()
                   .North(15);

            return builder.Tiles()
                          .ToList();
        }
    }
}
