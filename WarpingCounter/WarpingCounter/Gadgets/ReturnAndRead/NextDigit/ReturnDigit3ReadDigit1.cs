namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnDigit3ReadDigit1 : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 1;

        private readonly int tilesPerDigit;


        public readonly List<Tile> Tiles;


        public ReturnDigit3ReadDigit1(bool carry, int bits)
        {
            tilesPerDigit = bits * 4;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit3ReadDigit1)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit3ReadDigit1(carry);

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
                 .South(3)
                 .Down()
                 .South(14)
                 .South(tilesPerDigit)
                 .South(11)
                 .South()
                 .Up()
                 .West()
                 .South(2)
                 .East()
                 .South(16)
                 .South(tilesPerDigit)
                 .South(2)
                 .West()
                 .Down()
                 .South(28)
                 .South(tilesPerDigit)
                 .South(30)
                 .South(tilesPerDigit)
                 .South(30)
                 .South(tilesPerDigit)
                 .South()
                 .West();

            return build.Tiles()
                        .ToList();
        }

    }

}
