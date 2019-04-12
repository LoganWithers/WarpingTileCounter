namespace WarpingCounter.Gadgets.DigitTop
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using ReturnAndRead.NextDigit;


    /// <summary>
    ///   A gadget that is used only for the first digit in a MSR when it's case 2.
    ///   <br />
    ///   This digit top is special in that its tiles are mostly assembled in the z=0 plane,
    ///   contrary to a standard digit top.
    /// 
    ///   <br />
    ///   The last tile of this gadget connects to <see cref="ReturnDigit1ReadDigit2Case2" />
    /// </summary>
    /// <seealso cref="IHaveLast" />
    /// <seealso cref="IHaveFirst" />
    public class DigitTopDigit1Case2 : IHaveFirst, IHaveLast
    {
        
        private readonly int bitsPerDigit;

        public readonly List<Tile> Tiles;


        public DigitTopDigit1Case2(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;
            Tiles             = InitializeTiles();
            Tiles.PrependNamesWith($"{nameof(DigitTopDigit1Case2)} {carry}");

            First       = Tiles.First();
            First.South = GlueFactory.DigitTopDigit1Case2(carry);

            Last       = Tiles.Last();
            Last.South = GlueFactory.ReturnDigit1ReadDigit2Case2(carry);
        }


        public Tile First { get; }


        public Tile Last { get; }


        private List<Tile> InitializeTiles()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(4)
                   .Up()
                   .North()
                   .North()
                   .West()
                   .Down()
                   .North()
                   .West();

            builder.South(7);
            builder.SouthLine(bitsPerDigit);

            return builder.Tiles()
                          .ToList();
        }

    }

}
