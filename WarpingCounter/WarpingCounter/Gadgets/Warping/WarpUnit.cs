namespace WarpingCounter.Gadgets.Warping
{

    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    /// <summary>
    /// The warp unit tiles are the tiles which assemble upon reading one full digit in a counter value. 
    /// 
    /// This unit carries the following information:
    ///     next digit to write
    ///     increment or copy signal
    ///     index (within the current digit-region, i.e., 1, 2, or 3)
    ///     
    /// </summary>
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

            Tiles = InitializeTiles();
            First = Tiles.First();
        }


        public Tile First { get; }


        private List<Tile> InitializeTiles()
        {
            var preFirstWarp = new PreFirstWarp(bits, digitIndex, carry, digitsInMSR);
            var firstWarp    = new FirstWarp(bits,    digitIndex, carry, digitsInMSR);
            var warpBridge   = new WarpBridge(bits,   digitIndex, carry, digitsInMSR);
            var secondWarp   = new SecondWarp(bits,   digitIndex, carry, digitsInMSR);
            var postWarp     = new PostWarp(bits,     digitIndex, carry, digitsInMSR);

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
