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
    public class WarpUnit : IHaveInput
    {

        private readonly string digitValueToWrite;

        private readonly bool carry;

        private readonly int digitIndex;

        private readonly int digitsInMSR;

        public readonly List<Tile> Tiles;


        public WarpUnit(string digitValueToWrite, int digitIndex, bool carry, int digitsInMSR)
        {
            this.digitIndex        = digitIndex;
            this.digitValueToWrite = digitValueToWrite;
            this.carry             = carry;
            this.digitsInMSR       = digitsInMSR;

            Tiles = InitializeTiles();
            Input = Tiles.First();
        }


        public Tile Input { get; }


        private List<Tile> InitializeTiles()
        {
            var preFirstWarp = new PreWarp(digitValueToWrite,      digitIndex, carry, digitsInMSR);
            var firstWarp    = new FirstWarp(digitValueToWrite,    digitIndex, carry, digitsInMSR);
            var warpBridge   = new WarpBridge(digitValueToWrite,   digitIndex, carry, digitsInMSR);
            var secondWarp   = new SecondWarp(digitValueToWrite,   digitIndex, carry, digitsInMSR);
            var postWarp     = new PostWarp(digitValueToWrite,     digitIndex, carry, digitsInMSR);

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
