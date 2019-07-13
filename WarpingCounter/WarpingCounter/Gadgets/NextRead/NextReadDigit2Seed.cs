namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit2Seed : IHaveInput, IHaveOutput
    {
        private readonly int L;
        public Tile Input { get; }

        public Tile Output { get; }

        public IEnumerable<Tile> Tiles { get; }
        public NextReadDigit2Seed(string name, int L, Glue input, Glue output)
        {
            this.L = L;

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

            builder.East(2);

            return builder.Tiles().ToList();
        }
    }


}
