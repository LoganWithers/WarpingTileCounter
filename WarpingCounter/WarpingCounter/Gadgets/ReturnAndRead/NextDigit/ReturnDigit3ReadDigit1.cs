﻿namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
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


        public readonly List<Tile> Tiles;


        public ReturnDigit3ReadDigit1(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;

            Tiles = CreateTiles();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit3ReadDigit1)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit3ReadDigit1(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> CreateTiles()
        {
            var build = new GadgetBuilder().Start();

            build.South(12);

            build.West();

            build.South(3)
                 .Down();

            build.South(14);

            build.SouthLine(bitsPerDigit);

            build.South(11);

            build.South()
                 .Up()
                 .West()
                 .South()
                 .South()
                 .East();

            build.South(16);

            build.SouthLine(bitsPerDigit);

            build.South()
                 .South()
                 .West()
                 .Down();

            build.South(28);

            build.SouthLine(bitsPerDigit);

            build.South(30);

            build.SouthLine(bitsPerDigit);

            build.South(30);

            build.SouthLine(bitsPerDigit);

            build.South()
                 .West();

            return build.Tiles()
                        .ToList();
        }

    }

}
