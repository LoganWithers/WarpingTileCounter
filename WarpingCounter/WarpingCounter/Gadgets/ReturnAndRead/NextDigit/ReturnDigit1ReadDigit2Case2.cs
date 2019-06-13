namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class ReturnDigit1ReadDigit2Case2 : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 2;

        private readonly int L;

        public readonly List<Tile> Tiles;


        public ReturnDigit1ReadDigit2Case2(bool carry, int bits)
        {
            L = bits * 4;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit1ReadDigit2Case2)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit1ReadDigit2Case2(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South(29)
                   .South(L)
                   .South(30)
                   .South(L)
                   .South()
                   .Up()
                   .East(3)
                   .Down();

            return builder.Tiles()
                          .ToList();
        }

    }

}
