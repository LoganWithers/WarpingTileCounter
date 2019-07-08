namespace WarpingCounter.Common.Models
{

    public struct Coordinates
    {
        public Coordinates(int x = 0, int y = 0, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private int x { get; }


        private int y { get; }


        private int z { get; }


        public override string ToString() => $"{x} {y} {z}";

    }

}
