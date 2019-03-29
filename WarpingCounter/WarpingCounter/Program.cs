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
                        var initialValueManager = new SeedCreator( baseM, input);

                        if (initialValueManager.constructionDetails.DigitRegions < 2)
                        {
                            Error("Starting value must be result in >= 2 digit regions");
                            continue;
                        }

                        var tiles = initialValueManager.Tiles;
                        AddCounterTiles(tiles, initialValueManager.constructionDetails);
                        Write($"ThinRectangle_b{baseM}_from_{input}", tiles);

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
            List<Tile> uniqueTiles = tiles.DistinctBy(t => t.Name).ToList();

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

        private static void AddCounterTiles(List<Tile> tiles, ConstructionDetails construction)
        {
            var counter = new ReaderFactory(construction.ActualBitsPerDigit, construction.BitsPerCounterDigit, construction.BaseM);
            tiles.AddRange(counter.Readers.SelectMany(reader => reader.Tiles));
            tiles.AddRange(CreateReturnAndRead(construction.ActualBitsPerDigit, construction.DigitRegions));
            tiles.AddRange(CreateDigitTops(construction.ActualBitsPerDigit));
     

            Console.WriteLine($"Bits Per Counter Digit: {construction.BitsPerCounterDigit}");
            Console.WriteLine($"Actual Bits Per Digit:  {construction.ActualBitsPerDigit}");
            var fullSizeDigits = counter.UniqueDigits.Where(d => d.Length == construction.ActualBitsPerDigit).ToList();

            Console.WriteLine($"Full Digits: {fullSizeDigits.Count}");
            foreach (var binaryString in fullSizeDigits)
            {
                tiles.AddRange(CreateWarpUnits(binaryString, construction.DigitsInMSR));
                tiles.AddRange(CreateWriters(binaryString, construction.DigitsInMSR));
            }

        }


        private static IEnumerable<Tile> CreateDigitTops(int bitsPerDigit)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new DigitTopDefault(true, i, bitsPerDigit).Tiles);
                results.AddRange(new DigitTopDefault(false, i, bitsPerDigit).Tiles);
            }

            results.AddRange(new MsdDigitTop(true, bitsPerDigit).Tiles);
            results.AddRange(new MsdDigitTop(false, bitsPerDigit).Tiles);
            results.AddRange(new MsdDigitTopCase2(false, bitsPerDigit).Tiles);
            results.AddRange(new MsdDigitTopCase2(true, bitsPerDigit).Tiles);
            results.AddRange(new DigitTopDigit1Case2(true, bitsPerDigit).Tiles);
            results.AddRange(new DigitTopDigit1Case2(false, bitsPerDigit).Tiles);

            return results;
        }


        private static IEnumerable<Tile> CreateReturnAndRead(int bitsPerDigit, int regions)
        {
            var results = new List<Tile>();

            foreach (var carry in new[] { true, false })
            {
                var returnD1ReadD2      = new ReturnDigit1ReadDigit2(carry, bitsPerDigit);
                var returnD2ReadD3      = new ReturnDigit2ReadDigit3(carry, bitsPerDigit);
                var returnD3ReadD1      = new ReturnDigit3ReadDigit1(carry, bitsPerDigit);
                var returnD1ReadD2Case2 = new ReturnDigit1ReadDigit2Case2(carry, bitsPerDigit);

                var returnD1ReadD1      = new ReturnDigit1ReadNextRow(carry, bitsPerDigit, regions);
                var returnD2CrossReadD1 = new ReturnDigit2ReadNextRow(carry, bitsPerDigit, regions);
                var returnD3CrossReadD1 = new ReturnDigit3ReadNextRow(carry, bitsPerDigit, regions);

                results.AddRange(returnD2CrossReadD1.Tiles);
                results.AddRange(returnD1ReadD2Case2.Tiles);
                results.AddRange(returnD3CrossReadD1.Tiles);
                results.AddRange(returnD3ReadD1.Tiles);
                results.AddRange(returnD1ReadD2.Tiles);
                results.AddRange(returnD2ReadD3.Tiles);
                results.AddRange(returnD1ReadD1.Tiles);
            }

            return results;
        }

        private static IEnumerable<Tile> CreateWriters(string bits, int digitsInMSR)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new BinaryWriter(bits, true, i, digitsInMSR).Tiles);
                results.AddRange(new BinaryWriter(bits, false, i, digitsInMSR).Tiles);
            }

            return results;
        }


        private static IEnumerable<Tile> CreateWarpUnits(string bits, int digitsInMSR)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {

                results.AddRange(new WarpUnit(bits, i, true, digitsInMSR).Tiles);
                results.AddRange(new WarpUnit(bits, i, false, digitsInMSR).Tiles);

            }

            return results;
        }

    }

    
}
