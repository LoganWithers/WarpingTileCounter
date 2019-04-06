namespace WarpingCounter.Gadgets.Warping
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    public class WarpUnit : IHaveFirst
    {

        private readonly string bits;

        private readonly bool carry;

        private readonly int digitIndex;

        private readonly int digitsInMSR;

        public readonly List<Tile> Tiles;


        public WarpUnit(string bits, int digitIndex, bool carry, int digitsInMSR)
        {
            this.digitIndex  = digitIndex;
            this.bits        = bits;
            this.carry       = carry;
            this.digitsInMSR = digitsInMSR;

            Tiles = InitTiles();
            First = Tiles.First();
        }


        public Tile First { get; }


        private List<Tile> InitTiles()
        {
            var preFirstWarp = new PreFirstWarp(bits, digitIndex, carry, digitsInMSR);
            var firstWarp    = new FirstWarp(bits, digitIndex, carry, digitsInMSR);
            var warpBridge   = new WarpBridge(bits, digitIndex, carry, digitsInMSR);
            var secondWarp   = new SecondWarp(bits, digitIndex, carry, digitsInMSR);
            var postWarp     = new PostWarp(bits, digitIndex, carry, digitsInMSR);

            var tiles = new List<Tile>();

            tiles.AddRange(preFirstWarp.Tiles);
            tiles.Add(firstWarp.Tile);
            tiles.AddRange(warpBridge.Tiles);
            tiles.Add(secondWarp.Tile);
            tiles.AddRange(postWarp.Tiles);

            return tiles;
        }

    }

}
