namespace WarpingCounter.Gadgets.Warping.PreWarp
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class PreWarpDigit1 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public readonly List<Tile> Tiles;
        
        public PreWarpDigit1(Glue input, Glue output)
        {
            Tiles = Create();
            Tiles.PrependNamesWith(nameof(PreWarpDigit1));

            Input        = Tiles.First();
            Input.South  = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        public List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(17)
                   .West()
                   .North(3)
                   .Up()
                   .North()
                   .West()
                   .North()
                   .Down()
                   .North(7);

            return builder.Tiles().ToList();
        }
    }

}
