namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    using Common;
    using Common.IO;
    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.ReturnAndRead.NextDigit;
    using Gadgets.ReturnAndRead.NextRow;
    using Gadgets.Warping;

    using Seed;

    internal static class Program
    {

        private const int Digits = 3;


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
                  
                        //List<Tile> tiles = seedCreator.Tiles;
                        //AddCounterTiles(tiles, seedCreator.Construction);
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


        private static void AddCounterTiles(List<Tile> tiles, ConstructionValues construction)
        {
            var inMSR   = construction.DigitsInMSR;
            var counter = new ReaderFactory(construction.ActualBitsPerDigit, construction.BitsPerCounterDigit, construction.BaseM, construction.DigitsInMSR);
            tiles.AddRange(counter.Readers.SelectMany(reader => reader.Tiles));
            tiles.AddRange(CreateReturnAndRead(construction.ActualBitsPerDigit, construction.DigitRegions, inMSR));
            tiles.AddRange(CreateDigitTops(construction.ActualBitsPerDigit, inMSR));

            Console.WriteLine($"Bits Per Counter Digit: {construction.BitsPerCounterDigit}");
            Console.WriteLine($"Actual Bits Per Digit:  {construction.ActualBitsPerDigit}");

            List<string> fullSizeDigits = counter.digitsThatCanBeRead.Where(d => d.Length == construction.ActualBitsPerDigit)
                                                 .ToList();

            Console.WriteLine($"Full Digits: {fullSizeDigits.Count}");

            foreach (var binaryString in fullSizeDigits)
            {
                tiles.AddRange(CreateWarpUnits(binaryString, construction.DigitsInMSR));
                tiles.AddRange(CreateWriters(binaryString, construction.DigitsInMSR));
            }
        }


        private static IEnumerable<Tile> CreateDigitTops(int bitsPerDigit, int digitsInMSR)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new DigitTopDefault(true,  i, bitsPerDigit).Tiles);
                results.AddRange(new DigitTopDefault(false, i, bitsPerDigit).Tiles);
            }

            foreach (var carry in new[] {true, false})
            {
                switch (digitsInMSR)
                {
                    case 1:

                        break;

                    case 2:

                        results.AddRange(new DigitTopDigit2Case2(carry, bitsPerDigit).Tiles);
                        results.AddRange(new DigitTopDigit1Case2(carry, bitsPerDigit).Tiles);

                        break;

                    case 3:
                        results.AddRange(new DigitTopDigit3Case3(carry, bitsPerDigit).Tiles);

                        break;
                }
            }

            return results;
        }


        private static IEnumerable<Tile> CreateReturnAndRead(int bitsPerDigit, int regions, int digitsInMSR)
        {
            var results = new List<Tile>();

            foreach (var carry in new[] {true, false})
            {
                var returnD1ReadD2 = new ReturnDigit1ReadDigit2(carry, bitsPerDigit);
                var returnD2ReadD3 = new ReturnDigit2ReadDigit3(carry, bitsPerDigit);
                var returnD3ReadD1 = new ReturnDigit3ReadDigit1(carry, bitsPerDigit);

                results.AddRange(returnD3ReadD1.Tiles);
                results.AddRange(returnD1ReadD2.Tiles);
                results.AddRange(returnD2ReadD3.Tiles);

                switch (digitsInMSR)
                {
                    case 1:
                        var returnDigit1ReadNextRow = new ReturnDigit1ReadNextRow(carry, bitsPerDigit, regions);
                        results.AddRange(returnDigit1ReadNextRow.Tiles);

                        break;
                    case 2:
                        var returnDigit2ReadNextRow     = new ReturnDigit2ReadNextRow(carry, bitsPerDigit, regions);
                        var returnDigit1ReadDigit2Case2 = new ReturnDigit1ReadDigit2Case2(carry, bitsPerDigit);
                        results.AddRange(returnDigit1ReadDigit2Case2.Tiles);
                        results.AddRange(returnDigit2ReadNextRow.Tiles);

                        break;
                    case 3:
                        var returnDigit3ReadNextRow = new ReturnDigit3ReadNextRow(carry, bitsPerDigit, regions);
                        results.AddRange(returnDigit3ReadNextRow.Tiles);
                        results.AddRange(new DigitTopDigit3Case3(carry, bitsPerDigit).Tiles);

                        break;
                }
            }

            return results;
        }


        private static IEnumerable<Tile> CreateWriters(string bits, int digitsInMSR)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new BinaryWriter(bits, true,  i, digitsInMSR).Tiles);
                results.AddRange(new BinaryWriter(bits, false, i, digitsInMSR).Tiles);
            }

            return results;
        }


        private static IEnumerable<Tile> CreateWarpUnits(string bits, int digitsInMSR)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new WarpUnit(bits, i, true,  digitsInMSR).Tiles);
                results.AddRange(new WarpUnit(bits, i, false, digitsInMSR).Tiles);
            }

            return results;
        }

    }

}
