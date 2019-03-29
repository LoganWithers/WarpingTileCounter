namespace WarpingCounter.Gadgets.Warping
{

    using Common.Models;

    public class SecondWarp
    {

        public readonly Tile Tile;


        public SecondWarp(string bits, int index, bool carry, int digitsInMSR)
        {
            Tile = new Tile($"{nameof(SecondWarp)} bits={bits} index={index} carry={carry}")
            {
                North = GlueFactory.SecondWarp(bits, index, carry),
                South = GlueFactory.SecondWarp(bits, index, carry)
            };

            if (bits.EndsWith("11") && digitsInMSR == 2)
            {
                Tile.East = GlueFactory.PostWarp(bits, index, carry);
            } else
            {
                Tile.Up = GlueFactory.PostWarp(bits, index, carry);
            }

        }



    }
}
