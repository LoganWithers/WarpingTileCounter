namespace WarpingCounter.Gadgets.DigitTop
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    /// <summary>
    /// 
    /// The first tile of this gadget is connected to the last tile of a digit when it ends with "00".
    ///
    /// The last tile is connected to a return and read gadget.
    /// </summary>
    /// <seealso cref="IHaveLast" />
    /// <seealso cref="IHaveFirst" />
    public class DigitTopDefault : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;
        
        public Tile Last  { get; }
        public Tile First { get; }

        /// <summary>
        /// The current carry signal of the counter
        /// </summary>
        private readonly bool carry;

        private readonly int bitsPerDigit;

        /// <summary>
        /// The index of the digit that attached to the first tile of this gadget.
        /// <br/>
        /// Used in order to determine the correct output glue.
        /// </summary>
        private readonly int index;


        public DigitTopDefault(bool carry, int index, int bitsPerDigit)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;
            this.index        = index;

            Tiles       = InitializeTiles();
            Tiles.PrependNamesWith($"DigitTop {this.carry} {this.index}");

            First       = Tiles.First();
            First.South = GlueFactory.DigitTopDefault(carry, index);

            Last        = Tiles.Last();
            Last.South  = GetNextDigitToRead();
        }

        
        private Glue GetNextDigitToRead()
        {
            switch (index)
            {
                case 1:
                    return GlueFactory.ReturnDigit1ReadDigit2(carry);
                case 2:
                    return GlueFactory.ReturnDigit2ReadDigit3(carry);
                case 3:
                    return GlueFactory.ReturnDigit3ReadDigit1(carry);
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

            return build.SouthLine(bitsPerDigit, carry)
                        .Tiles()
                        .ToList();

        }

    }

}

