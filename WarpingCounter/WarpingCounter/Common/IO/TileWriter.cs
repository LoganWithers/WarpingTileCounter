namespace WarpingCounter.Common.IO
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Models;

    public class TileWriter
    {
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
            Console.WriteLine($".tdp file can be found at {path}");
        }

        private void WriteDefinitions()
        {
            var path = $"{GetPath()}{options.SimpleTileSetName}.tds";
            File.WriteAllText(path, string.Join("\n", tiles.Select(t => t.ToString())));
            Console.WriteLine($".tds file can be found at {path}\n\n");
        }

        private const string ProjectFolder = "\\WarpingCounter\\WarpingCounter\\";

        private string GetPath()
        {
            var currentDirectory = Environment.CurrentDirectory;

            var systemIndependentPathPrefix = currentDirectory.IndexOf(ProjectFolder, StringComparison.OrdinalIgnoreCase);

            return $"{currentDirectory.Substring(0, systemIndependentPathPrefix)}{ProjectFolder}Output\\";
        }
        
    }
}
