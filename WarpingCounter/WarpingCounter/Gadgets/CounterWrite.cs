namespace WarpingCounter.Gadgets
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class CounterWrite0 : IHaveInput, IHaveOutput
    {
        public readonly List<Tile> Tiles;

        public Tile Input { get; }
        public Tile Output { get; }
        public CounterWrite0(string name, Glue input, Glue output)
        {
            Tiles       = Create();
            Tiles.RenameWithIndex(name);
            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = output;
        }

        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(2)
                   .East()
                   .North()
                   .West()
                   .North();

            return builder.Tiles().Skip(1).ToList();
        }
    }

    public class CounterWrite1 : IHaveInput, IHaveOutput
    {
        public readonly List<Tile> Tiles;
        public Tile Input { get; }
        public Tile Output { get; }

        public CounterWrite1(string name, Glue input, Glue output)
        {
            Tiles       = Create();
            Tiles.RenameWithIndex(name);
            Input       = Tiles.First();
            Input.South = input;

            Output       = Tiles.Last();
            Output.North = output;
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(2)
                   .Up()
                   .East()
                   .North()
                   .West()
                   .Down()
                   .North();

            return builder.Tiles().Skip(1).ToList();
        }
    }
}
