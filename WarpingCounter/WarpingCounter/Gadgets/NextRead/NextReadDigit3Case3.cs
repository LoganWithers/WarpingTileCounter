namespace WarpingCounter.Gadgets.NextRead
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextReadDigit3Case3 : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }
        public Tile Output { get; }

        public IEnumerable<Tile> Tiles { get; }
        public NextReadDigit3Case3(string name, Glue input, Glue output)
        {
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

            builder.South()
                   .West()
                   .Down()
                   .South(2)
                   .Up()
                   .East(5);


            return builder.Tiles().ToList();
        }
    }


}
