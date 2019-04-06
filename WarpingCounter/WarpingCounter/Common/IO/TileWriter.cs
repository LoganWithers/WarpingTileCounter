namespace WarpingCounter.Common.IO
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Models;

    public class TileWriter
    {

        private const string ProjectFolder = "\\WarpingCounter\\WarpingCounter\\";

        private readonly TdpOptions options;

        private readonly IEnumerable<Tile> tiles;


        public TileWriter(TdpOptions options, IEnumerable<Tile> tiles)
        {
            this.options = options;
            this.tiles   = tiles;
        }


        public void WriteTileSet()
        {
            WriteOptions();
            WriteDefinitions();
        }


        private void WriteOptions()
        {
            var path = $"{GetPath()}{options.SimpleTileSetName}.tdp";
            File.WriteAllText(path, options.ToString());
            Console.WriteLine($"File can be found at {options.SimpleTileSetName}.tdp");
        }


        private void WriteDefinitions()
        {
            var path = $"{GetPath()}{options.SimpleTileSetName}.tds";
            var sb   = new StringBuilder();

            foreach (var tile in tiles)
            {
                sb.AppendLine(tile.ToString());
            }

            File.WriteAllText(path, sb.ToString());
        }


        private string GetPath()
        {
            var currentDirectory = Environment.CurrentDirectory;

            var systemIndependentPathPrefix = currentDirectory.IndexOf(ProjectFolder, StringComparison.OrdinalIgnoreCase);

            return $"{currentDirectory.Substring(0, systemIndependentPathPrefix)}{ProjectFolder}Output\\";
        }

    }

}
