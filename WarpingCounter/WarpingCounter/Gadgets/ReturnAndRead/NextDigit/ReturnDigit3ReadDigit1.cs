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

        private readonly int bitsPerDigit;

        private readonly bool carry;

        public readonly List<Tile> Tiles;


        public ReturnDigit3ReadDigit1(bool carry, int bitsPerDigit)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;

            Tiles = InitTiles();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit3ReadDigit1)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit3ReadDigit1(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> InitTiles()
        {
            var build = new GadgetBuilder().Start();

            build.South(12);

            build.West();

            build.South(3)
                 .Down();

            build.South(14);

            build.SouthLine(bitsPerDigit, carry);

            build.South(11);

            build.South()
                 .Up()
                 .West()
                 .South()
                 .South()
                 .East();

            build.South(16);

            build.SouthLine(bitsPerDigit, carry);

            build.South()
                 .South()
                 .West()
                 .Down();

            build.South(28);

            build.SouthLine(bitsPerDigit, carry);

            build.South(30);

            build.SouthLine(bitsPerDigit, carry);

            build.South(30);

            build.SouthLine(bitsPerDigit, carry);

            build.South()
                 .West();

            return build.Tiles()
                        .ToList();
        }

    }

}
