namespace WarpingCounter.Gadgets.Warping
{

    using System;

    using Common.Models;

    public class FirstWarp
    {

        private readonly string bits;

        private readonly bool carry;

        private readonly int digitsInMSR;

        private readonly int index;


        private readonly string name;

        public readonly Tile Tile;


        public FirstWarp(string bits, int index, bool carry, int digitsInMSR)
        {
            this.bits        = bits;
            this.index       = index;
            this.carry       = carry;
            this.digitsInMSR = digitsInMSR;
            name             = $"{nameof(FirstWarp)} bits={bits} index={index} carry={carry}";

            Tile = Init();
        }


        private Tile Init()
        {
            switch (digitsInMSR)
            {
                case 3:

                    return CreateForThreeDigits();

                case 2:

                {
                    // Not in the MSR
                    if (bits.EndsWith("00"))
                    {
                        return CreateForThreeDigits();
                    }

                    // Digit 1 in the MSR
                    if (bits.EndsWith("01"))
                    {
                        return CreateDigit1Case2();
                    }

                    // Digit 2 (MSD) in the MSR
                    if (bits.EndsWith("11"))
                    {
                        return CreateDigit2Case2();
                    }

                    throw new ArgumentOutOfRangeException(bits);
                }

                case 1:

                    return bits.EndsWith("11") ? CreateDigit1Case1() // Digit 1 in the MSR
                    : CreateForThreeDigits();                        // Not in the MSR

                default:

                    throw new ArgumentOutOfRangeException(nameof(digitsInMSR));
            }
        }


        private Tile CreateForThreeDigits() => new Tile(name) {
        North = GlueFactory.FirstWarp(bits, index, carry),
        South = GlueFactory.FirstWarp(bits, index, carry),
        East  = GlueFactory.WarpBridge(bits, index, carry)
        };


        private Tile CreateDigit1Case2() => new Tile(name) {
        North = GlueFactory.FirstWarp(bits, index, carry),
        South = GlueFactory.FirstWarp(bits, index, carry),

        // Digit 1 warps past where digit 2 and 3 usually are (in a standard region), thus
        // skipping a warp bridge and a second warp tile.
        East = GlueFactory.PostWarp(bits, index, carry)
        };


        private Tile CreateDigit2Case2() => new Tile(name) {
        North = GlueFactory.FirstWarp(bits, index, carry),
        South = GlueFactory.FirstWarp(bits, index, carry),

        // Digit 2 begins warping right away after the tiles are read.
        // Its warp bridge should assemble just before the crossing region
        West = GlueFactory.WarpBridge(bits, index, carry)
        };


        private Tile CreateDigit1Case1() => new Tile(name) {
        North = GlueFactory.FirstWarp(bits, index, carry),
        South = GlueFactory.FirstWarp(bits, index, carry),

        // First warp tile is the only warp tile in case 1
        Up = GlueFactory.PostWarp(bits, index, carry)
        };

    }

}
