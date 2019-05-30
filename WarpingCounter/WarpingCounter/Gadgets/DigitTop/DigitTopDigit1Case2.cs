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
    /// <seealso cref="IHaveOutput" />
    /// <seealso cref="IHaveInput" />
    public class DigitTopDigit1Case2 : IHaveInput, IHaveOutput
    {
        
        private readonly int tilesPerDigit;

        public readonly List<Tile> Tiles;


        public DigitTopDigit1Case2(bool carry, int bits)
        {
            tilesPerDigit = bits * 4;
            Tiles         = Create();
            Tiles.PrependNamesWith($"{nameof(DigitTopDigit1Case2)} {carry}");

            Input       = Tiles.First();
            Input.South = GlueFactory.DigitTopDigit1Case2(carry);

            Output       = Tiles.Last();
            Output.South = GlueFactory.ReturnDigit1ReadDigit2Case2(carry);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.North(4)
                   .Up()
                   .North()
                   .North()
                   .West()
                   .Down()
                   .North()
                   .West()
                   .South(7)
                   .South(tilesPerDigit);

            return builder.Tiles()
                          .ToList();
        }

    }

}
