namespace WarpingCounter.Gadgets.ReturnPath
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnPathDigit1Seed : IHaveInput, IHaveOutput
    {
        public Tile Input { get; }
        
        public Tile Output { get; }
        
        public List<Tile> Tiles { get; }
        
        public ReturnPathDigit1Seed(string name, Glue input, Glue output)
        {

            Tiles = Create();
            Tiles.RenameWithIndex(name);

            Input       = Tiles.First();
            Input.Up    = input;

            Output       = Tiles.Last();
            Output.North = output;
        }

        /// <summary>
        /// 1 tile
        /// </summary>
        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();
            
            return builder.Tiles().ToList();
        }

    }

}
