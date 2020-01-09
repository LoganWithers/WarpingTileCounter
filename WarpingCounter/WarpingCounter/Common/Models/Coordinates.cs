namespace WarpingCounter.Common.Models
{
    public struct Coordinates
    {
        public Coordinates(int x = 0, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private int X { get; }

        private int Y { get; }

        private int Z { get; }

        public override string ToString() => $"{X} {Y} {Z}";
    }
}
