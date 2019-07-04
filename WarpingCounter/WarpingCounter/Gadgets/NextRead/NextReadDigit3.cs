namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit3 : IHaveInput, IHaveOutput
    {

        private readonly int L;
        public Tile Input { get; }
        public Tile Output { get; }

        public IEnumerable<Tile> Tiles { get; }

        public NextReadDigit3(int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.PrependNamesWith(nameof(NextReadDigit3));

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(2)
                   .West()
                   .Down()
                   .South(28)
                   .South(4 * L, "blue")
                   .South(30)
                   .South(4 * L, "blue")
                   .South(30)
                   .South(4 * L, "blue")
                   .South()
                   .West();

            return builder.Tiles().ToList();
        }
    }


}
