namespace WarpingCounter.Gadgets
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Builders;
    using Common.Models;

    public class RoofUnit : IHaveInput, IHaveOutput
    {

        public Tile Input { get; }


        public Tile Output { get; }


        public readonly List<Tile> Tiles;


        private readonly int digitsInMSR;
        private readonly int L;


        public RoofUnit(int digitsInMSR, int digits, int L, Glue input, bool kIsOdd)
        {
            this.digitsInMSR = digitsInMSR;
            this.L = L;

            Output = new Tile("Roof");
            Tiles  = Create();
            Tiles.Add(Output);

            Input = Tiles.First();
            Input.South = input;

            var difference     = digits % 3 == 0 ? -1 : 0;
            var generalRegions = (int)Math.Floor((decimal)digits / 3) + difference;

            var builder = new GadgetBuilder().Start();
            for (var i = 0; i < generalRegions; i++)
            {
                builder.East(6);
            }

            if (kIsOdd)
            {
                builder.East();
            }

            var tiles = builder.Tiles().Skip(1).ToList();
            tiles.First().AttachWest(Output);
            SpawnFiller(Output);
            tiles.ForEach(SpawnFiller);
            Tiles.AddRange(tiles);
            Tiles.RenameWithIndex("Roof");
        }


        private List<Tile> Create()
        {
            var builder = new GadgetBuilder().Start();

            switch (digitsInMSR)
            {
                case 1:
                    builder.North(29)
                           .North(4 * L, "blue")
                           .North(30)
                           .North(4 * L, "blue")
                           .North(30);
                    break;

                case 2:
                    builder.North(29)
                           .North(4 * L, "blue")
                           .North(30);
                    break;

                case 3:
                    builder.North(29);
                    break;
            }

            var tiles = builder.Tiles().ToList();

            var last = tiles.Last();

            var leftWall = AddWestWall();
            tiles.AddRange(leftWall);

            last.AttachEast(Output);

            if (leftWall.Any())
            {
                last.AttachWest(leftWall.First());
            }

            return tiles;
        }


        private List<Tile> AddWestWall()
        {
            var builder = new GadgetBuilder().Start();
            switch (digitsInMSR)
            {
                case 2:
                    builder.West(2);
                    break;

                case 3:
                    builder.West(4);
                    break;
            }

            var tiles = builder.Tiles().Skip(1).ToList();

            tiles.ForEach(SpawnFiller);

            return tiles;
        }


        private void SpawnFiller(Tile tile)
        {
            tile.South = new Glue("Filler");
        }
    }
}
