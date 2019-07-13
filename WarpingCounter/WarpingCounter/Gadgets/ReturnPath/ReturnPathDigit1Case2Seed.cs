namespace WarpingCounter.Gadgets.ReturnPath
{

    using System.Collections.Generic;
    using System.Linq;

    using Common.Builders;
    using Common.Models;

    public class ReturnPathDigit1Case2Seed
    {
        private readonly int L;


        public Tile Input { get; }


        public Tile Output { get; }


        public IEnumerable<Tile> Tiles { get; }


        public ReturnPathDigit1Case2Seed(string name, int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.East  = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            return builder.Tiles().ToList();
        }

    }
}
