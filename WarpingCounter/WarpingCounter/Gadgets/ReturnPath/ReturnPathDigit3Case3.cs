namespace WarpingCounter.Gadgets.ReturnPath
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnPathDigit3Case3 : IHaveInput, IHaveOutput
    {

        private readonly int L;


        public Tile Input { get; }


        public Tile Output { get; }


        public IEnumerable<Tile> Tiles { get; }


        public ReturnPathDigit3Case3(int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.PrependNamesWith(nameof(ReturnPathDigit3Case3));

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.South = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(12)
                   .West()
                   .South(3)
                   .Down()
                   .South(14)
                   .South(4 * L, "blue")
                   .South(12)
                   .Up()
                   .West()
                   .South(2)
                   .East()
                   .South(16)
                   .South(4 * L, "blue");

            return builder.Tiles().ToList();
        }

    }

}
