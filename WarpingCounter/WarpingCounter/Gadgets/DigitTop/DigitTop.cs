namespace WarpingCounter.Gadgets.DigitTop
{

    using Common;
    using Common.Builders;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   The first tile of this gadget is connected to the last tile of a digit when it ends with "00".
    ///   The last tile is connected to a return and read gadget.
    /// </summary>
    public class DigitTop : IHaveInput, IHaveOutput
    {
        public readonly List<Tile> Tiles;

        private readonly int bitsPerDigit;

        /// <summary>
        ///   The current carry signal of the counter
        /// </summary>
        private readonly bool carry;

        /// <summary>
        ///   The index of the digit that attached to the first tile of this gadget.
        ///   <br />
        ///   Used in order to determine the correct output glue.
        /// </summary>
        private readonly int index;



        public DigitTop(bool carry, int index, int bitsPerDigit)
        {
            this.carry = carry;
            this.bitsPerDigit = bitsPerDigit;
            this.index = index;

            Tiles = InitializeTiles();
            Tiles.PrependNamesWith($"DigitTop {this.carry} {this.index}");

            Input = Tiles.First();
            Input.South = GlueFactory.DigitTop(carry, index);

            Output = Tiles.Last();
            Output.South = GetNextDigitToRead();
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private Glue GetNextDigitToRead()
        {
            switch (index)
            {
                case 1: return GlueFactory.ReturnDigit1ReadDigit2(carry);
                case 2: return GlueFactory.ReturnDigit2ReadDigit3(carry);
                case 3: return GlueFactory.ReturnDigit3ReadDigit1(carry);
                default: 
                    throw new ArgumentOutOfRangeException($"Invalid digit index: {index}");
            }
        }


        private List<Tile> InitializeTiles()
        {
            var build = new GadgetBuilder().Start();

            build.North(4)
                 .Up();

            build.North(10)
                 .West()
                 .West()
                 .Down()
                 .South()
                 .South()
                 .South()
                 .East()
                 .South()
                 .West()
                 .South()
                 .East()
                 .South()
                 .South()
                 .South()
                 .Up()
                 .North()
                 .West();

            build.South(7);

            return build.SouthLine(bitsPerDigit)
                        .Tiles()
                        .ToList();
        }

    }

}
