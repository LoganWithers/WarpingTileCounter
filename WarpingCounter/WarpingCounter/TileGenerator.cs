namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common;
    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;
    using Gadgets.Warping.FirstWarp;
    using Gadgets.Warping.PostWarp;
    using Gadgets.Warping.PreWarp;
    using Gadgets.Warping.SecondWarp;
    using Gadgets.Warping.WarpBridge;

    using Seed;

    /// <summary>
    /// Provided the details of a counter, this creates all the gadgets needed to
    /// assemble a counter with a specific base and starting value. Namely, the
    /// following "gadgets" will be created.
    ///
    /// 
    /// </summary>
    public class TileGenerator : AbstractTileNamer
    {

        private const int Digits = 3;

        private readonly ConstructionValues construction;

        private readonly List<Tile> tiles = new List<Tile>();

        private readonly int digitsInMSR;

        private readonly int L;

        private readonly int logM;

        private readonly int regions;

        private readonly int M;


        public TileGenerator(int m, string initialValueB10)
        {
            M            = m;
            construction = new ConstructionValues(initialValueB10, m);
            digitsInMSR  = construction.DigitsInMSR;
            L            = construction.L;
            logM         = construction.BitsRequiredForBaseM;
            regions      = construction.DigitRegions;

            Console.WriteLine($"Bits Per Counter Digit: {logM}");
            Console.WriteLine($"Actual Bits Per Digit:  {L}");
        }


        public bool IsStartingValueTooSmall() => construction.DigitRegions < 2;

        public (string name, List<Tile> tileset) Generate()
        {
            const string gadget = PostWarp;
            const int    i      = 3;
            const bool   op     = true;
            const bool   msr    = true;
            const bool   msd    = true;
            string       name   = $"{gadget} {i} {op} {msr} {msd}";

            tiles.Add(new Tile("seed") {North = new Glue($"{CounterWrite} {1} {Seed} 0 {1} ")});

            var digits = new BinaryStringPermutations(L, logM, M).Permutations.ToList();

            CreateSeed();
            CreateCounterRead(digits);

            var fullDigits = new HashSet<string>();

            foreach (var digit in digits.Where(d => d.Length == L - 1))
            {
                fullDigits.Add($"0{digit}");
                fullDigits.Add($"1{digit}");
            }

            CreatePreWarp(fullDigits);
            CreateFirstWarp(fullDigits);
            CreateWarpBridge(fullDigits);
            CreateSecondWarp(fullDigits);
            CreatePostWarp(fullDigits);

            CreateCounterWrite(digits);

            CreateDigitTops();
            CreateNextRead();
            CreateReturnPaths();
            CreateCrossNextRow();
            var before = tiles.Count;

            var after = tiles.DistinctBy(t => t.Name)
                             .ToList();

            Console.Write($"Found {before - after.Count} duplicate tiles");

            return (name.Replace(" ", "_")
                        .ToLower(), after);
        }


        private void CreateSeed()
        {
            tiles.AddRange(new SeedCreator(construction).Tiles);
        }


        string Name(string gadget, int i, string U, bool op, bool msr = false, bool msd = false) => $"{gadget} {i} '{U}' {op} {msr} {msd}";


        string Name(string gadget, int i, bool op, bool msr = false, bool msd = false) => $"{gadget} {i} {op} {msr} {msd}";


        string Name(string gadget, bool op) => $"{gadget} {op}";


        private void CreateCounterWrite(IReadOnlyCollection<string> digits)
        {
            for (var i = 1; i <= Digits; i++)
            {
                var digitsBeforeMSB = digits.Where(digit => 1 <= digit.Length && digit.Length <= L - 1)
                                            .OrderBy(s => s.Length)
                                            .ThenBy(s => s)
                                            .ToList();

                foreach (var op in new[] {true, false})
                {
                    foreach (var U in digitsBeforeMSB)
                    {
                        tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, $"{U}0", op),
                                                         Bind(CounterWrite, i, $"{U}0", op),
                                                         Bind(CounterWrite, i, U,       op)).Tiles);

                        tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, $"{U}1", op),
                                                         Bind(CounterWrite, i, $"{U}1", op),
                                                         Bind(CounterWrite, i, U,       op)).Tiles);

                        tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, $"{U}0", op, msr: true),
                                                         Bind(CounterWrite, i, $"{U}0", op, msr: true),
                                                         Bind(CounterWrite, i, U,       op, msr: true)).Tiles);

                        tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, $"{U}1", op, msr: true),
                                                         Bind(CounterWrite, i, $"{U}1", op, msr: true),
                                                         Bind(CounterWrite, i, U,       op, msr: true)).Tiles);

                        tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, $"{U}0", op, msr: true, msd: true),
                                                         Bind(CounterWrite, i, $"{U}0", op, msr: true, msd: true),
                                                         Bind(CounterWrite, i, U,       op, msr: true, msd: true)).Tiles);

                        tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, $"{U}1", op, msr: true, msd: true),
                                                         Bind(CounterWrite, i, $"{U}1", op, msr: true, msd: true),
                                                         Bind(CounterWrite, i, U,       op, msr: true, msd: true)).Tiles);
                    }

                    tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, "0", op),
                                                     Bind(CounterWrite, i, "0", op),
                                                     Bind(DigitTop,     i,      op)).Tiles);

                    tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, "1", op),
                                                     Bind(CounterWrite, i, "1", op),
                                                     Bind(DigitTop,     i,      op)).Tiles);

                    tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, "0", op, msr: true),
                                                     Bind(CounterWrite, i, "0", op, msr: true),
                                                     Bind(DigitTop,     i,      op, msr: true)).Tiles);

                    tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, "1", op, msr: true),
                                                     Bind(CounterWrite, i, "1", op, msr: true),
                                                     Bind(DigitTop,     i,      op, msr: true)).Tiles);

                    tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, "0", op, msr: true, msd: true),
                                                     Bind(CounterWrite, i, "0", op, msr: true, msd: true),
                                                     Bind(DigitTop,     i,      op, msr: true, msd: true)).Tiles);

                    tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, "1", op, msr: true, msd: true),
                                                     Bind(CounterWrite, i, "1", op, msr: true, msd: true),
                                                     Bind(DigitTop,     i,      op,  msr: true, msd: true)).Tiles);
                }
            }
        }


        private (Glue out0, Glue out1) ReadMostSignificantBit(string U, int i)
        {
            var guess0 = CalculateOutput($"0{U}", i);
            var guess1 = CalculateOutput($"1{U}", i);

            return (guess0, guess1);
        }


        private Glue CalculateOutput(string U, int i)
        {
            const int binary = 2;

            var indicatorBits = U.GetLast(2);
            var valueBits     = U.Substring(0, U.Length - 2);

            int ConvertToDecimal(string guess) => Convert.ToInt32(guess, binary);

            string ConvertToBinary(int value) => Convert.ToString(value, binary)
                                                        .PadLeft(valueBits.Length, '0');

            var zeroes = string.Concat(Enumerable.Repeat("0", valueBits.Length));

            if (ConvertToDecimal(valueBits) + 1 <= M - 1)
            {
                return Bind(Names.PreWarp, i, ConvertToBinary(ConvertToDecimal(valueBits) + 1) + indicatorBits, op: false);
            }

            if (indicatorBits == "11")
            {
                return Bind(Names.PreWarp, i, $"{zeroes}11", op: false); // TODO: change op: false to op: "halt"
            }

            return Bind(Names.PreWarp, i, zeroes + indicatorBits, op: true);
        }


        private void CreateCounterRead(IReadOnlyCollection<string> digits)
        {
            for (var i = 1; i <= Digits; i++)
            {
                var digitsBeforeMSB = digits.Where(digit => digit.Length <= L - 2)
                                            .OrderBy(s => s.Length)
                                            .ThenBy(s => s)
                                            .ToList();

                foreach (var op in new[] {true, false})
                {
                    foreach (var U in digitsBeforeMSB)
                    {
                        tiles.AddRange(new CounterRead(Name(CounterRead, i, U,       op),
                                                       Bind(CounterRead, i, U,       op),
                                                       Bind(CounterRead, i, $"1{U}", op),
                                                       Bind(CounterRead, i, $"0{U}", op)).Tiles);
                    }
                }

                var digitsForMSB = digits.Where(digit => digit.Length == L - 1)
                                         .OrderBy(s => s.Length)
                                         .ThenBy(s => s)
                                         .ToList();

                foreach (var U in digitsForMSB)
                {
                    tiles.AddRange(new CounterRead(Name(CounterRead, i, U,       op: false),
                                                   Bind(CounterRead, i, U,       op: false),
                                                   Bind(PreWarp,     i, $"1{U}", op: false),
                                                   Bind(PreWarp,     i, $"0{U}", op: false)).Tiles);
                }

                foreach (var U in digitsForMSB)
                {
                    var (out0, out1) = ReadMostSignificantBit(U, i);

                    tiles.AddRange(new CounterRead(Name(CounterRead, i, U, op: true),
                                                   Bind(CounterRead, i, U, op: true),
                                                   outputOne: out1,
                                                   outputZero: out0).Tiles);
                }
            }
        }


        private void CreatePreWarp(IEnumerable<string> digits)
        {
            foreach (var U in digits)
            {
                foreach (var op in new[] {true, false})
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new PreWarpDigit1(Name(PreWarp,   1, U, op),
                                                             Bind(PreWarp,   1, U, op),
                                                             Bind(FirstWarp, 1, U, op)).Tiles);

                            tiles.AddRange(new PreWarpDigit2(Name(PreWarp,   2, U, op),
                                                             Bind(PreWarp,   2, U, op),
                                                             Bind(FirstWarp, 2, U, op)).Tiles);

                            tiles.AddRange(new PreWarpDigit3(Name(PreWarp,   3, U, op),
                                                             Bind(PreWarp,   3, U, op),
                                                             Bind(FirstWarp, 3, U, op)).Tiles);

                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new PreWarpDigit1Case2(Name(PreWarp,   1, U, op, msr: true),
                                                                  Bind(PreWarp,   1, U, op),
                                                                  Bind(FirstWarp, 1, U, op, msr: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new PreWarpDigit1Case1(Name(PreWarp,   1, U, op, msr: true, msd: true),
                                                                  Bind(PreWarp,   1, U, op),
                                                                  Bind(FirstWarp, 1, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new PreWarpDigit2Case2(Name(PreWarp,   2, U, op, msr: true, msd: true),
                                                                  Bind(PreWarp,   2, U, op),
                                                                  Bind(FirstWarp, 2, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new PreWarpDigit3Case3(Name(PreWarp,   3, U, op, msr: true, msd: true),
                                                                  Bind(PreWarp,   3, U, op),
                                                                  Bind(FirstWarp, 3, U, op, msr: true, msd: true)).Tiles);

                            break;

                        default: continue;
                    }
                }
            }
        }


        private void CreateFirstWarp(IEnumerable<string> digits)
        {
            foreach (var U in digits)
            {
                foreach (var op in new[] {true, false})
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new FirstWarpDigit1(Name(FirstWarp,  1, U, op),
                                                               Bind(FirstWarp,  1, U, op),
                                                               Bind(FirstWarp,  1, U, op),
                                                               Bind(WarpBridge, 1, U, op)).Tiles);

                            tiles.AddRange(new FirstWarpDigit2(Name(FirstWarp,  2, U, op),
                                                               Bind(FirstWarp,  2, U, op),
                                                               Bind(FirstWarp,  2, U, op),
                                                               Bind(WarpBridge, 2, U, op)).Tiles);

                            tiles.AddRange(new FirstWarpDigit3(Name(FirstWarp,  3, U, op),
                                                               Bind(FirstWarp,  3, U, op),
                                                               Bind(FirstWarp,  3, U, op),
                                                               Bind(WarpBridge, 3, U, op)).Tiles);

                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new FirstWarpDigit1Case2(Name(FirstWarp, 1, U, op, msr: true),
                                                                    Bind(FirstWarp, 1, U, op, msr: true),
                                                                    Bind(FirstWarp, 1, U, op, msr: true),
                                                                    Bind(PostWarp,  1, U, op, msr: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new FirstWarpDigit1Case1(Name(FirstWarp, 1, U, op, msr: true, msd: true),
                                                                    Bind(FirstWarp, 1, U, op, msr: true, msd: true),
                                                                    Bind(FirstWarp, 1, U, op, msr: true, msd: true),
                                                                    Bind(PostWarp,  1, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new FirstWarpDigit2Case2(Name(FirstWarp,  2, U, op, msr: true, msd: true),
                                                                    Bind(FirstWarp,  2, U, op, msr: true, msd: true),
                                                                    Bind(FirstWarp,  2, U, op, msr: true, msd: true),
                                                                    Bind(WarpBridge, 2, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new FirstWarpDigit3Case3(Name(FirstWarp,  3, U, op, msr: true, msd: true),
                                                                    Bind(FirstWarp,  3, U, op, msr: true, msd: true),
                                                                    Bind(FirstWarp,  3, U, op, msr: true, msd: true),
                                                                    Bind(WarpBridge, 3, U, op, msr: true, msd: true)).Tiles);

                            break;

                        default: continue;
                    }
                }
            }
        }


        private void CreateWarpBridge(IEnumerable<string> digits)
        {
            foreach (var U in digits)
            {
                foreach (var op in new[] {true, false})
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new WarpBridgeDigit1(Name(WarpBridge, 1, U, op),
                                                                Bind(WarpBridge, 1, U, op),
                                                                Bind(SecondWarp, 1, U, op)).Tiles);

                            tiles.AddRange(new WarpBridgeDigit2(Name(WarpBridge, 2, U, op),
                                                                Bind(WarpBridge, 2, U, op),
                                                                Bind(SecondWarp, 2, U, op)).Tiles);

                            tiles.AddRange(new WarpBridgeDigit3(Name(WarpBridge, 3, U, op),
                                                                Bind(WarpBridge, 3, U, op),
                                                                Bind(SecondWarp, 3, U, op)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new WarpBridgeDigit2Case2(Name(WarpBridge, 2, U, op, msr: true, msd: true),
                                                                     Bind(WarpBridge, 2, U, op, msr: true, msd: true),
                                                                     Bind(SecondWarp, 2, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new WarpBridgeDigit3Case3(Name(WarpBridge, 3, U, op, msr: true, msd: true),
                                                                     Bind(WarpBridge, 3, U, op, msr: true, msd: true),
                                                                     Bind(SecondWarp, 3, U, op, msr: true, msd: true)).Tiles);

                            break;

                        default: continue;
                    }
                }
            }
        }


        private void CreateSecondWarp(IEnumerable<string> digits)
        {
            foreach (var U in digits)
            {
                foreach (var op in new[] {true, false})
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new SecondWarpDigit1(Name(SecondWarp, 1, U, op),
                                                                Bind(SecondWarp, 1, U, op),
                                                                Bind(SecondWarp, 1, U, op),
                                                                Bind(PostWarp,   1, U, op)).Tiles);

                            tiles.AddRange(new SecondWarpDigit2(Name(SecondWarp, 2, U, op),
                                                                Bind(SecondWarp, 2, U, op),
                                                                Bind(SecondWarp, 2, U, op),
                                                                Bind(PostWarp,   2, U, op)).Tiles);

                            tiles.AddRange(new SecondWarpDigit3(Name(SecondWarp, 3, U, op),
                                                                Bind(SecondWarp, 3, U, op),
                                                                Bind(SecondWarp, 3, U, op),
                                                                Bind(PostWarp,   3, U, op)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new SecondWarpDigit2Case2(Name(SecondWarp, 2, U, op, msr: true, msd: true),
                                                                     Bind(SecondWarp, 2, U, op, msr: true, msd: true),
                                                                     Bind(SecondWarp, 2, U, op, msr: true, msd: true),
                                                                     Bind(PostWarp,   2, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new SecondWarpDigit3Case3(Name(SecondWarp, 3, U, op, msr: true, msd: true),
                                                                     Bind(SecondWarp, 3, U, op, msr: true, msd: true),
                                                                     Bind(SecondWarp, 3, U, op, msr: true, msd: true),
                                                                     Bind(PostWarp,   3, U, op, msr: true, msd: true)).Tiles);

                            break;

                        default:
                            continue;

                            ;
                    }
                }
            }
        }


        private void CreatePostWarp(IEnumerable<string> digits)
        {
            foreach (var U in digits)
            {
                foreach (var op in new[] {true, false})
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new PostWarpDigit1(Name(PostWarp,     1, U, op),
                                                              Bind(PostWarp,     1, U, op),
                                                              Bind(CounterWrite, 1, U, op)).Tiles);

                            tiles.AddRange(new PostWarpDigit2(Name(PostWarp,     2, U, op),
                                                              Bind(PostWarp,     2, U, op),
                                                              Bind(CounterWrite, 2, U, op)).Tiles);

                            tiles.AddRange(new PostWarpDigit3(Name(PostWarp,     3, U, op),
                                                              Bind(PostWarp,     3, U, op),
                                                              Bind(CounterWrite, 3, U, op)).Tiles);

                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new PostWarpDigit1Case2(Name(PostWarp,     1, U, op, msr: true),
                                                                   Bind(PostWarp,     1, U, op, msr: true),
                                                                   Bind(CounterWrite, 1, U, op, msr: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new PostWarpDigit1Case1(Name(PostWarp,     1, U, op, msr: true, msd: true),
                                                                   Bind(PostWarp,     1, U, op, msr: true, msd: true),
                                                                   Bind(CounterWrite, 1, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new PostWarpDigit2Case2(Name(PostWarp,     2, U, op, msr: true, msd: true),
                                                                   Bind(PostWarp,     2, U, op, msr: true, msd: true),
                                                                   Bind(CounterWrite, 2, U, op, msr: true, msd: true)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new PostWarpDigit3Case3(Name(PostWarp,     3, U, op, msr: true, msd: true),
                                                                   Bind(PostWarp,     3, U, op, msr: true, msd: true),
                                                                   Bind(CounterWrite, 3, U, op, msr: true, msd: true)).Tiles);

                            break;

                        default: continue;
                    }
                }
            }
        }


        private void CreateDigitTops()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new DigitTop(Name(DigitTop,   1, op),
                                            L,
                                            Bind(DigitTop,   1, op),
                                            Bind(ReturnPath, 1, op)).Tiles);

                tiles.AddRange(new DigitTop(Name(DigitTop,   2, op),
                                            L,
                                            Bind(DigitTop,   2, op),
                                            Bind(ReturnPath, 2, op)).Tiles);

                tiles.AddRange(new DigitTop(Name(DigitTop,   3, op),
                                            L,
                                            Bind(DigitTop,   3, op),
                                            Bind(ReturnPath, 3, op)).Tiles);

                switch (digitsInMSR)
                {
                    case 1:
                        tiles.AddRange(new DigitTopDigit1Case1(Name(DigitTop,   1, op, msr: true, msd: true),
                                                               L,
                                                               Bind(DigitTop,   1, op, msr: true, msd: true),
                                                               Bind(ReturnPath, 1, op, msr: true, msd: true)).Tiles);

                        break;

                    case 2:
                        tiles.AddRange(new DigitTopDigit1Case2(Name(DigitTop,   1, op, msr: true),
                                                               L,
                                                               Bind(DigitTop,   1, op, msr: true),
                                                               Bind(ReturnPath, 1, op, msr: true)).Tiles);

                        tiles.AddRange(new DigitTopDigit2Case2(Name(DigitTop,   2, op, msr: true, msd: true),
                                                               L,
                                                               Bind(DigitTop,   2, op, msr: true, msd: true),
                                                               Bind(ReturnPath, 2, op, msr: true, msd: true)).Tiles);

                        break;

                    case 3:
                        tiles.AddRange(new DigitTop(Name(DigitTop,   3, op, msr: true, msd: true),
                                                    L,
                                                    Bind(DigitTop,   3, op, msr: true, msd: true),
                                                    Bind(ReturnPath, 3, op, msr: true, msd: true)).Tiles);

                        break;
                }
            }
        }


        private void CreateReturnPaths()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new ReturnPathDigit1(Name(ReturnPath, 1, op),
                                                    L,
                                                    Bind(ReturnPath, 1, op),
                                                    Bind(NextRead,   1, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit1Case2(Name(ReturnPath, 1, op, msr: true),
                                                         L,
                                                         Bind(ReturnPath, 1, op, msr: true),
                                                         Bind(NextRead,   1, op, msr: true)).Tiles);

                tiles.AddRange(new ReturnPathDigit1Case1(Name(ReturnPath, 1, op, msr: true, msd: true),
                                                         L,
                                                         Bind(ReturnPath, 1, op, msr: true, msd: true),
                                                         Bind(NextRead,   1, op, msr: true, msd: true)).Tiles);

                tiles.AddRange(new ReturnPathDigit2(Name(ReturnPath, 2, op),
                                                    L,
                                                    Bind(ReturnPath, 2, op),
                                                    Bind(NextRead,   2, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit2Case2(Name(ReturnPath, 2, op, msr: true, msd: true),
                                                         L,
                                                         Bind(ReturnPath, 2, op, msr: true, msd: true),
                                                         Bind(NextRead,   2, op, msr: true, msd: true)).Tiles);

                tiles.AddRange(new ReturnPathDigit3(Name(ReturnPath, 3, op),
                                                    L,
                                                    Bind(ReturnPath, 3, op),
                                                    Bind(NextRead,   3, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit3Case3(Name(ReturnPath, 3, op, msr: true, msd: true),
                                                         L,
                                                         Bind(ReturnPath, 3, op, msr: true, msd: true),
                                                         Bind(NextRead,   3, op, msr: true, msd: true)).Tiles);
            }
        }


        private void CreateNextRead()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new NextReadDigit1(Name(NextRead,    1,               op),
                                                  L,                                 
                                                  Bind(NextRead,    1,               op),
                                                  Bind(CounterRead, 2, string.Empty, op)).Tiles);

                tiles.AddRange(new NextReadDigit1Case2(Name(NextRead,    1,               op, msr: true),
                                                       L,                                 
                                                       Bind(NextRead,    1,               op, msr: true),
                                                       Bind(CounterRead, 2, string.Empty, op, msr: true)).Tiles);

                tiles.AddRange(new NextReadDigit1Case1(Name(NextRead,     1, op, msr: true, msd: true),
                                                       L,
                                                       Bind(NextRead,     1, op, msr: true, msd: true),
                                                       Bind(CrossNextRow,    op)).Tiles);

                tiles.AddRange(new NextReadDigit2(Name(NextRead,    2,               op),
                                                  L,                                 
                                                  Bind(NextRead,    2,               op),
                                                  Bind(CounterRead, 3, string.Empty, op)).Tiles);

                tiles.AddRange(new NextReadDigit2Case2(Name(NextRead,     2, op, msr: true, msd: true),
                                                       L,
                                                       Bind(NextRead,     2, op, msr: true, msd: true),
                                                       Bind(CrossNextRow, op)).Tiles);

                tiles.AddRange(new NextReadDigit3(Name(NextRead,    3,               op),
                                                  L,                                 
                                                  Bind(NextRead,    3,               op),
                                                  Bind(CounterRead, 1, string.Empty, op)).Tiles);

                tiles.AddRange(new NextReadDigit3Case3(Name(NextRead,     3, op, msr: true, msd: true),
                                                       L,
                                                       Bind(NextRead,     3, op, msr: true, msd: true),
                                                       Bind(CrossNextRow, op)).Tiles);
            }
        }


        private void CreateCrossNextRow()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new CrossNextRow(Name(CrossNextRow,                  op),
                                                construction.d,                     
                                                Bind(CrossNextRow,                  op),
                                                Bind(CounterRead,  1, string.Empty, op)).Tiles);
            }
        }

    }

}
