namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit1Case1 : IHaveInput, IHaveOutput
    {

        private readonly int L;
        public Tile Input { get; }
        public Tile Output { get; }

        public IEnumerable<Tile> Tiles { get; }
        public NextReadDigit1Case1(int L, Glue input, Glue output)
        {
            this.L = L;

            Tiles = Create();
            Tiles.PrependNamesWith(nameof(NextReadDigit1Case1));

            Input       = Tiles.First();
            Input.North = input;

            Output       = Tiles.Last();
            Output.East  = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(10)
                   .West()
                   .South(4)
                   .East()
                   .South(15)
                   .South(4 * L, "blue")
                   .South()
                   .West()
                   .South(3);

            return builder.Tiles().ToList();
        }
    }


}
