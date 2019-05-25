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

        private readonly int bitsPerDigit;
        
        public readonly List<Tile> Tiles;


        public ReturnDigit1ReadDigit2(bool carry, int bitsPerDigit)
        {
            this.bitsPerDigit = bitsPerDigit;

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
                   .East()
                   .East()
                   .South()
                   .Down()
                   .South()
                   .South()
                   .Up()
                   .East()
                   .South()
                   .South()
                   .Down()
                   .South()
                   .South()
                   .South()
                   .South()
                   .South()
                   .Up()
                   .South()
                   .West()
                   .West()
                   .West()
                   .West()
                   .South()
                   .South()
                   .South()
                   .Down();

            builder.South(14);

            builder.SouthLine(bitsPerDigit);

            builder.South(15)
                   .East()
                   .Up()
                   .East()
                   .East()
                   .East();

            builder.South(11);

            builder.South()
                   .West()
                   .West()
                   .South()
                   .South()
                   .South();

            builder.SouthLine(bitsPerDigit);

            builder.South()
                   .East()
                   .East()
                   .Down();

            return builder.Tiles()
                          .ToList();
        }

    }

}
