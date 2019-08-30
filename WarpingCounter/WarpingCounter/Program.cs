namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Globalization;
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


        private static (int baseM, string startingValue, int d) CalculateCounterInputs(double N, double k)
        {
            // ⌊k/2⌋ 
            var d = Math.Floor(k / 2);

            // ⌈(N/102)^(1/d)⌉
            var m = Math.Ceiling(Math.Pow(N / 102, 1 / d));


            // ⌈log m⌉ + 2
            var l = Math.Ceiling(Math.Log(m, 2)) + 2;
            
            // m^d 
            var md = Math.Pow(m, d);

            // s = m^d - ⌊ (N - 12l - 94) / (12l + 90) ⌋
            var s = md - Math.Floor((N - 12*l - 94) / (12*l + 90));


            return ((int) m, Convert.ToString((int) s, CultureInfo.InvariantCulture), (int) d);
        }

        private static void RunCLI()
        {
            while (true)
            {
                Log("Enter a value for N");
                var input = Console.ReadLine();
                bool IsExitCommand() => input == "-e" || string.IsNullOrEmpty(input);

                if (int.TryParse(input, out var N))
                {
                    Log("Enter a value for k");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out var k))
                    {
                        var (baseM, startingValue, d) = CalculateCounterInputs(N, k);

                        var generator = new TileGenerator($"{N}_x_{k}", baseM, startingValue, d, k % 2 == 1);

                        if (generator.IsStartingValueTooSmall())
                        {
                            Error("Starting value must be result in >= 2 digit regions");
                            continue;
                        }

                        var (name, tileSet) = generator.Generate();
                        Write(name, tileSet);
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
            var before = tiles.Count;

            var unique = tiles.DistinctBy(t => t.Name).ToHashSet();

            var after = unique.Count;

            if (before != after)
            {
                Error($"Found {before - after} duplicate tiles.");
            }
            
            var writer = new TileWriter(new TdpOptions(name), unique);

            writer.WriteTileSet();
        }


        private static void Log(string message)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
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
