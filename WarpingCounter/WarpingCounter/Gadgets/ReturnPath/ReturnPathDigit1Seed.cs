namespace WarpingCounter.Gadgets.ReturnPath
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnPathDigit1Seed : IHaveInput, IHaveOutput
    {

        private readonly int L;


        public Tile Input { get; }


        public Tile Output { get; }


        public IEnumerable<Tile> Tiles { get; }


        public ReturnPathDigit1Seed(int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.PrependNamesWith(nameof(ReturnPathDigit1Seed));

            Input       = Tiles.First();
            Input.Up    = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();
            
            return builder.Tiles().ToList();
        }

    }

}
