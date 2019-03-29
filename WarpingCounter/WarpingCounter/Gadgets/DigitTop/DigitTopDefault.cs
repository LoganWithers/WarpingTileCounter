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
    /// The first tile of this gadget is connected to the the north-most tile of a digit that is not the most significant digit in a region.
    ///
    /// The last tile is connected to a return and read gadget.
    /// </summary>
    /// <seealso cref="IHaveLast" />
    /// <seealso cref="IHaveFirst" />
    public class DigitTopDefault : IHaveFirst, IHaveLast
    {
        public readonly List<Tile> Tiles;
        
        public Tile Last { get; }
        public Tile First { get; }


        private readonly bool carry;

        private readonly int bitsPerDigit;

        private readonly int index;


        public DigitTopDefault(bool carry, int index, int bitsPerDigit)
        {
            this.carry        = carry;
            this.bitsPerDigit = bitsPerDigit;
            this.index        = index;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(DigitTopDefault)} carry={carry} index={index}");

            First = Tiles.First();
            First.South = GlueFactory.DigitTopStart(carry, index);

            Last  = Tiles.Last();
            Last.South  = BindToReturnGadgets();
        }


        // Need to add check for MSD 
        private Glue BindToReturnGadgets()
        {
            switch (index)
            {
                case 1:
                    return GlueFactory.ReturnD1ReadD2(carry);
                case 2:
                    return GlueFactory.ReturnD2ReadD3(carry);
                case 3:
                    return GlueFactory.ReturnD3ReadD1(carry);
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

