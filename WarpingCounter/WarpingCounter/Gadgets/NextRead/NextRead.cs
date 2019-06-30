namespace WarpingCounter.Gadgets.NextRead
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class NextRead : IHaveInput, IHaveOutput
    {

        private readonly int L;
        public Tile Input { get; }
        public Tile Output { get; }

        public IEnumerable<Tile> Tiles { get; }
        public NextRead(int L, int i, bool op, bool msr, bool msd)
        {
            this.L = L;
        }


        private List<Tile> CreateMsrMsd(int i, bool op)
        {
            var builder = new GadgetBuilder().Start();

            switch (i)
            {
                case 1:
                case 2:
                    builder.South(15)
                           .East()
                           .Up()
                           .East(3)
                           .South(11)
                           .South()
                           .West(2)
                           .South(3)
                           .South(4 * L)
                           .South()
                           .East(2)
                           .Down();
                    break;
                case 3:
                {
                    builder.South(2)
                           .West()
                           .Down()
                           .South(28)
                           .South(4 * L)
                           .South(30)
                           .South(4 * L)
                           .South(30)
                           .South(4 * L)
                           .South()
                           .West();
                        break;
                }
                    
            }

            var tiles = builder.Tiles().ToList();

            tiles.First().North = GlueFactory.Create(Names.NextRead, i, op);
            tiles.Last().North  = GlueFactory.Create(Names.CrossNextRow, i, op);

            return builder.Tiles().ToList();
        }


        private List<Tile> Create(int i, bool op, bool msr)
        {
            var builder = new GadgetBuilder();


            return builder.Tiles().ToList();
        }


        private List<Tile> Create(int i, bool op, bool msr, bool msd)
        {
            var builder = new GadgetBuilder();


            return builder.Tiles().ToList();
        }

    }


}
