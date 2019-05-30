namespace WarpingCounter.Gadgets.ReturnAndRead.NextDigit
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    using DigitTop;

    /// <summary>
    ///   Gadget that occurs in a standard 3 digit region. Attached after digit 1 and its corresponding top is placed.
    ///   The first tile of this gadget is connected to the last tile of a <see cref="DigitTop" /> gadget.
    ///   The last tile of this gadget binds to an empty digit 2 reader.
    /// </summary>
    /// <seealso cref="IHaveInput" />
    /// <seealso cref="IHaveOutput" />
    public class ReturnDigit1ReadDigit2 : IHaveInput, IHaveOutput
    {

        private const int NextDigitRead = 2;

        private readonly int tilesPerDigit;
        
        public readonly List<Tile> Tiles;


        public ReturnDigit1ReadDigit2(bool carry, int bits)
        {
            tilesPerDigit = bits * 4;

            Tiles = Create();
            Tiles.PrependNamesWith($"{nameof(ReturnDigit1ReadDigit2)} {carry}");

            Input       = Tiles.First();
            Input.North = GlueFactory.ReturnDigit1ReadDigit2(carry);

            Output       = Tiles.Last();
            Output.North = GlueFactory.DigitReader(string.Empty, carry, NextDigitRead);
        }


        public Tile Input { get; }


        public Tile Output { get; }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            builder.South()
                   .East(2)
                   .South()
                   .Down()
                   .South(2)
                   .Up()
                   .East()
                   .South(2)
                   .Down()
                   .South(5)
                   .Up()
                   .South()
                   .West(4)
                   .South(3)
                   .Down()
                   .South(14)
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
                   .Down();

            return builder.Tiles()
                          .ToList();
        }

    }

}
