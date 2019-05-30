namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnDigit2ReadDigit3 : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 3;

        private readonly int tilesPerDigit;

        public readonly List<Tile> Tiles;


        public ReturnDigit2ReadDigit3(bool carry, int bits)
        {
            tilesPerDigit = bits * 4;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit2ReadDigit3)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit2ReadDigit3(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var build = new GadgetBuilder().Start();

            build.South(12)
                 .West()
                 .Down()
                 .South(17)
                 .South(tilesPerDigit)
                 .South(15)
                 .East()
                 .Up()
                 .East(3)
                 .South(11)
                 .South()
                 .West(2)
                 .South(3)
                 .South(tilesPerDigit)
                 .South()
                 .East(2)
                 .East()
                 .Down();

            return build.Tiles()
                        .ToList();
        }

    }

}
