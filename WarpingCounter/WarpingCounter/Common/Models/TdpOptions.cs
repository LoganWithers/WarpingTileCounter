namespace WarpingCounter.Common.Models
{
    using System.Text;

    public class TdpOptions
    {
        public TdpOptions(string tdsFileName, string seedName = "seed")
        {
            SimpleTileSetName = tdsFileName;
            SeedName          = seedName;
            Coordinates       = new Coordinates(1, 1, 1);
        }

        public string SimpleTileSetName { get; }

        private int Temperature { get; } = 1;

        private string SeedName { get; }

        private Coordinates Coordinates { get; }

        public override string ToString() => new StringBuilder()
                                            .Append($"{SimpleTileSetName}.tds")
                                            .AppendLine()
                                            .Append("Mode=aTAM")
                                            .AppendLine()
                                            .Append($"Temperature={Temperature}")
                                            .AppendLine()
                                            .Append($"{SeedName} {Coordinates.ToString()}")
                                            .ToString();
    }
}
