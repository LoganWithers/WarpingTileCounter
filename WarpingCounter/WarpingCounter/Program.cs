namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Common.IO;
    using Common.Models;


    internal static class Program
    {
        
        private static void Main(string[] args)
        {
            RunCLI();
        }


        private static (int baseM, string startingValue) CalculateCounterInputs(double N, double k)
        {
            var d = Math.Floor(k / 2);
            var m = Math.Ceiling(Math.Pow(N / 93, 1 / d));
            var l = Math.Ceiling(Math.Log(m, 2)) + 2;
            var md = Math.Pow(m, d);
            var s = md - Math.Floor((N - 3*l - 76) / (3*l + 90));
            
            return ((int) m, Convert.ToString((int) s, CultureInfo.InvariantCulture));
        }

        private static void RunCLI()
        {

            while (true)
            {
                Console.WriteLine("Enter a value for N");
                var input = "454541120";
                bool IsExitCommand() => input == "-e" || string.IsNullOrEmpty(input);

                if (int.TryParse(input, out var N))
                {
                    Console.WriteLine("Enter a value for k");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out var k))
                    {
                        var (baseM, startingValue) = CalculateCounterInputs(N, k);

                        var generator = new TileGenerator(baseM, startingValue);

                        if (generator.IsStartingValueTooSmall())
                        {
                            Error("Starting value must be result in >= 2 digit regions");
                            continue;
                        }

                        Write($"ThinRectangle_{N}x{k}_b={baseM}_c0={startingValue}", generator.Generate());
                        continue;
                    }
                }
                if (IsExitCommand())
                {
                   break;
                }

                Error($"Error parsing {input}...");
            }
        }


        private static void Write(string name, IReadOnlyCollection<Tile> tiles)
        {
            List<Tile> uniqueTiles = tiles.DistinctBy(t => t.Name)
                                          .ToList();

            Console.WriteLine($"Unique Tiles: {uniqueTiles.Count}");

            var writer = new TileWriter(new TdpOptions(name), uniqueTiles);

            writer.WriteTileSet();
        }


        private static void Error(string message)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
        }


        private static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

    }

}
