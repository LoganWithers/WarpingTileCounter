namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    using Common.IO;
    using Common.Models;


    internal static class Program
    {
        
        private static void Main(string[] args)
        {
            RunCLI();
        }


        private static void RunCLI()
        {
            while (true)
            {
                Console.WriteLine("Base value?");
                var input = Console.ReadLine();

                bool IsExitCommand() => input == "-e" || string.IsNullOrEmpty(input);

                if (int.TryParse(input, out var baseM) && baseM >= 2 && baseM <= 1000)
                {
                    Console.WriteLine("Starting value?");
                    input = Console.ReadLine();

                    if (BigInteger.TryParse(input ?? "", out _))
                    {
                        var generator = new TileGenerator(baseM, input);

                        if (generator.IsStartingValueTooSmall())
                        {
                            Error("Starting value must be result in >= 2 digit regions");

                            continue;
                        }
                  
                        Write($"WarpingCounter_b{baseM}_from_{input}", generator.Generate());

                        continue;
                    }

                    if (IsExitCommand())
                    {
                        break;
                    }

                    Error($"Error parsing {input}...");

                    continue;
                }

                if (IsExitCommand())
                {
                    break;
                }

                Error($"Error parsing {input}... make sure the base is a number between 2 and 1000");
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
