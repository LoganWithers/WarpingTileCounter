namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit3Seed : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }

        public Tile Output { get; }

        public IEnumerable<Tile> Tiles { get; }

        public NextReadDigit3Seed(string name, Glue input, Glue output)
        {

            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South()
                   .Down()
                   .West(3)
                   .North();

            return builder.Tiles().ToList();
        }
    }


}
