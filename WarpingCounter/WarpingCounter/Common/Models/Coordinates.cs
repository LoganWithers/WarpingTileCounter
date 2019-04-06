namespace WarpingCounter.Common.Models
{

    public struct Coordinates
    {

        private int x { get; }


        private int y { get; }


        private int z { get; }


        public override string ToString() => $"{x} {y} {z}";

    }

}
