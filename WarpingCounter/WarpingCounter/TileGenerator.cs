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
    public class TileGenerator
    {
        public const string CounterRead  = "CounterRead";
        public const string PreWarp      = "PreWarp";
        public const string FirstWarp    = "FirstWarp";
        public const string WarpBridge   = "WarpBridge";
        public const string SecondWarp   = "SecondWarp";
        public const string PostWarp     = "PostWarp";
        public const string CounterWrite = "CounterWrite";
        public const string DigitTop     = "DigitTop";
        public const string ReturnPath   = "ReturnPath";
        public const string NextRead     = "NextRead";
        public const string Seed         = "Seed";
        public const string CrossNextRow = "CrossNextRow";

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
            M                    = m;
            construction         = new ConstructionValues(initialValueB10, m);
            digitsInMSR          = construction.DigitsInMSR;
            L                    = construction.L;
            logM = construction.BitsRequiredForBaseM;
            regions              = construction.DigitRegions;

            Console.WriteLine($"Bits Per Counter Digit: {logM}");
            Console.WriteLine($"Actual Bits Per Digit:  {L}");
        }

        public bool IsStartingValueTooSmall() => construction.DigitRegions < 2;


        public (string name, List<Tile> tileset) Generate()
        {
            const string gadget = PostWarp;
            const int   i  = 3;
            const bool op  = true;
            const bool msr = true;
            const bool msd = true;
            string name = $"{gadget} {i} {op} {msr} {msd}";

            tiles.Add(new Tile("seed"){ North = new Glue($"{CounterWrite} {1} {Seed} 0 {1} ")});
            
            var readerFactory = new ReaderFactory(L, logM, M);
            var digits = new BinaryStringPermutations(L, logM, M).Permutations.ToList();

            CreateSeed();
            CreateCounterRead(digits);
            
            var fullDigits = new HashSet<string>();

            foreach (var digit in digits)
            {
                fullDigits.Add($"0{digit}");
                fullDigits.Add($"1{digit}");
            }

            CreatePreWarp(fullDigits);
            CreateFirstWarp(fullDigits);
            CreateWarpBridge(fullDigits);
            CreateSecondWarp(fullDigits);
            CreatePostWarp(fullDigits);

            CreateCounterWrite(fullDigits);

            CreateDigitTops();
            CreateNextRead();
            CreateReturnPaths();
            CreateCrossNextRow();
            var before = tiles.Count;
            var after = tiles.DistinctBy(t => t.Name).ToList();
            Console.Write($"Found {before - after.Count} duplicate tiles");
            return (name.Replace(" ", "_").ToLower(), after);
        }
        
        private void CreateSeed()
        {
            tiles.AddRange(new SeedCreator(construction).Tiles);
        }


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
                        tiles.AddRange(new CounterWrite0(GlueFactory.Create(CounterWrite, i, $"0{U}", op), 
                                                         GlueFactory.Create(CounterWrite, i,     U,   op)).Tiles);

                        tiles.AddRange(new CounterWrite1(GlueFactory.Create(CounterWrite, i, $"1{U}", op),
                                                         GlueFactory.Create(CounterWrite, i, U,       op)).Tiles);

                        tiles.AddRange(new CounterWrite0(GlueFactory.Create(CounterWrite, i, $"0{U}", op, msr: true),
                                                         GlueFactory.Create(CounterWrite, i, U,       op, msr: true)).Tiles);

                        tiles.AddRange(new CounterWrite1(GlueFactory.Create(CounterWrite, i, $"1{U}", op, msr: true),
                                                         GlueFactory.Create(CounterWrite, i, U,       op, msr: true)).Tiles);

                        tiles.AddRange(new CounterWrite0(GlueFactory.Create(CounterWrite, i, $"0{U}", op, msr: true, msd: true),
                                                         GlueFactory.Create(CounterWrite, i, U,       op, msr: true, msd: true)).Tiles);

                        tiles.AddRange(new CounterWrite1(GlueFactory.Create(CounterWrite, i, $"1{U}", op, msr: true, msd: true),
                                                         GlueFactory.Create(CounterWrite, i, U,       op, msr: true, msd: true)).Tiles);
                    }

                    tiles.AddRange(new CounterWrite0(GlueFactory.Create(CounterWrite, i, "0", op),
                                                     GlueFactory.Create(DigitTop, i, op)).Tiles);

                    tiles.AddRange(new CounterWrite1(GlueFactory.Create(CounterWrite, i, "1", op),
                                                     GlueFactory.Create(DigitTop,     i,      op)).Tiles);

                    tiles.AddRange(new CounterWrite0(GlueFactory.Create(CounterWrite, i, "0", op, msr: true),
                                                     GlueFactory.Create(DigitTop,     i,      op, msr: true)).Tiles);

                    tiles.AddRange(new CounterWrite1(GlueFactory.Create(CounterWrite, i, "1", op, msr: true),
                                                     GlueFactory.Create(DigitTop,     i,      op, msr: true)).Tiles);

                    tiles.AddRange(new CounterWrite0(GlueFactory.Create(CounterWrite, i, "0", op, msr: true, msd: true),
                                                     GlueFactory.Create(DigitTop,     i,      op, msr: true, msd: true)).Tiles);

                    tiles.AddRange(new CounterWrite1(GlueFactory.Create(CounterWrite, i, "1", op, msr: true, msd: true),
                                                     GlueFactory.Create(DigitTop,     i,      op, msr: true, msd: true)).Tiles);
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

            string ConvertToBinary(int value) => Convert.ToString(value, binary).PadLeft(valueBits.Length, '0');

            var zeroes = string.Concat(Enumerable.Repeat("0", valueBits.Length));

            if (ConvertToDecimal(valueBits) + 1 <= M - 1)
            {
                return GlueFactory.Create(Names.PreWarp, i, ConvertToBinary(ConvertToDecimal(valueBits) + 1) + indicatorBits, op: false);
            }

            if (indicatorBits == "11")
            {
                return GlueFactory.Create(Names.PreWarp, i, $"{zeroes}11", op: false); // TODO: change op: false to op: "halt"
            }

            return GlueFactory.Create(Names.PreWarp, i, zeroes + indicatorBits, op: true);
        }

        private void CreateCounterRead(IReadOnlyCollection<string> digits)
        {
            for (var i = 1; i <= Digits; i++)
            {
                var digitsBeforeMSB = digits.Where(digit => digit.Length <= L - 2)
                                            .OrderBy(s => s.Length)
                                            .ThenBy(s => s)
                                            .ToList();

                foreach (var op in new[] { true, false })
                {
                    foreach (var bits in digitsBeforeMSB)
                    {
                        tiles.AddRange(new CounterRead(GlueFactory.Create(CounterRead, i, bits,       op),
                                                       GlueFactory.Create(CounterRead, i, $"1{bits}", op),
                                                       GlueFactory.Create(CounterRead, i, $"0{bits}", op)).Tiles);
                    }
                }

                var digitsForMSB = digits.Where(digit => digit.Length == L - 1)
                                         .OrderBy(s => s.Length)
                                         .ThenBy(s => s)
                                         .ToList();

                foreach (var bits in digitsForMSB)
                {
                    tiles.AddRange(new CounterRead(GlueFactory.Create(CounterRead, i, bits,       op: false),
                                                   GlueFactory.Create(PreWarp,     i, $"1{bits}", op: false),
                                                   GlueFactory.Create(PreWarp,     i, $"0{bits}", op: false)).Tiles);
                }

                foreach (var bits in digitsForMSB)
                {
                    var (out0, out1) = ReadMostSignificantBit(bits, i);

                    tiles.AddRange(new CounterRead(GlueFactory.Create(CounterRead, i, bits, op: true),
                                                   outputOne: out1,
                                                   outputZero: out0).Tiles);
                }
            }
        }

        private void CreateCounter()
        {
            var readerFactory = new ReaderFactory(L, logM, M);
            tiles.AddRange(readerFactory.Readers.SelectMany(reader => reader.Tiles));

            List<string> fullSizeDigits = readerFactory.DigitsWithLengthL;


            Console.WriteLine($"Full Digits: {fullSizeDigits.Count}");

            // Foreach digit in {0, 1}^l
            foreach (var lengthLDigit in fullSizeDigits)
            {
                tiles.AddRange(CreateWriters(lengthLDigit));
            }
        }


        private void CreatePreWarp(IEnumerable<string> digits)
        {
            foreach (var u in digits)
            {
                foreach (var op in new[] { true, false })
                {
                    switch (u.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new PreWarpDigit1(GlueFactory.Create(PreWarp,   1, u, op), 
                                                             GlueFactory.Create(FirstWarp, 1, u, op)).Tiles);

                            tiles.AddRange(new PreWarpDigit2(GlueFactory.Create(PreWarp,   2, u, op),
                                                             GlueFactory.Create(FirstWarp, 2, u, op)).Tiles);

                            tiles.AddRange(new PreWarpDigit3(GlueFactory.Create(PreWarp,   3, u, op),
                                                             GlueFactory.Create(FirstWarp, 3, u, op)).Tiles);
                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new PreWarpDigit1Case2(GlueFactory.Create(PreWarp,   1, u, op), 
                                                                  GlueFactory.Create(FirstWarp, 1, u, op, msr: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new PreWarpDigit1Case1(GlueFactory.Create(PreWarp,   1, u, op), 
                                                                  GlueFactory.Create(FirstWarp, 1, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new PreWarpDigit2Case2(GlueFactory.Create(PreWarp,   2, u, op),
                                                                  GlueFactory.Create(FirstWarp, 2, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new PreWarpDigit3Case3(GlueFactory.Create(PreWarp,   3, u, op),
                                                                  GlueFactory.Create(FirstWarp, 3, u, op, msr: true, msd: true)).Tiles);
                            break;

                        default: continue;
                    }   
                }
            }
            
        }


        private void CreateFirstWarp(IEnumerable<string> digits)
        {
            foreach (var u in digits)
            {
                foreach (var op in new[] {true, false})
                {
                    switch (u.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new FirstWarpDigit1(GlueFactory.Create(FirstWarp,  1, u, op),
                                                               GlueFactory.Create(FirstWarp,  1, u, op),
                                                               GlueFactory.Create(WarpBridge, 1, u, op)).Tiles);


                            tiles.AddRange(new FirstWarpDigit2(GlueFactory.Create(FirstWarp,  2, u, op),
                                                               GlueFactory.Create(FirstWarp,  2, u, op),
                                                               GlueFactory.Create(WarpBridge, 2, u, op)).Tiles);


                            tiles.AddRange(new FirstWarpDigit3(GlueFactory.Create(FirstWarp,  3, u, op),
                                                               GlueFactory.Create(FirstWarp,  3, u, op),
                                                               GlueFactory.Create(WarpBridge, 3, u, op)).Tiles);
                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new FirstWarpDigit1Case2(GlueFactory.Create(FirstWarp, 1, u, op, msr: true),
                                                                    GlueFactory.Create(FirstWarp, 1, u, op, msr: true),
                                                                    GlueFactory.Create(PostWarp,  1, u, op, msr: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new FirstWarpDigit1Case1(GlueFactory.Create(FirstWarp, 1, u, op, msr: true, msd: true),
                                                                    GlueFactory.Create(FirstWarp, 1, u, op, msr: true, msd: true),
                                                                    GlueFactory.Create(PostWarp,  1, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new FirstWarpDigit2Case2(GlueFactory.Create(FirstWarp,  2, u, op, msr: true, msd: true),
                                                                    GlueFactory.Create(FirstWarp,  2, u, op, msr: true, msd: true),
                                                                    GlueFactory.Create(WarpBridge, 2, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new FirstWarpDigit3Case3(GlueFactory.Create(FirstWarp,  3, u, op, msr: true, msd: true),
                                                                    GlueFactory.Create(FirstWarp,  3, u, op, msr: true, msd: true),
                                                                    GlueFactory.Create(WarpBridge, 3, u, op, msr: true, msd: true)).Tiles);
                            break;

                        default: continue;
                    }
                }
            }
        }


        private void CreateWarpBridge(IEnumerable<string> digits)
        {
            foreach (var u in digits)
            {
                foreach (var op in new[] { true, false })
                {
                    switch (u.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new WarpBridgeDigit1(GlueFactory.Create(WarpBridge, 1, u, op),
                                                                GlueFactory.Create(SecondWarp, 1, u, op)).Tiles);

                            tiles.AddRange(new WarpBridgeDigit2(GlueFactory.Create(WarpBridge, 2, u, op),
                                                                GlueFactory.Create(SecondWarp, 2, u, op)).Tiles);

                            tiles.AddRange(new WarpBridgeDigit3(GlueFactory.Create(WarpBridge, 3, u, op),
                                                                GlueFactory.Create(SecondWarp, 3, u, op)).Tiles);
                            break;
                        
                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new WarpBridgeDigit2Case2(GlueFactory.Create(WarpBridge, 2, u, op, msr: true, msd: true),
                                                                     GlueFactory.Create(SecondWarp, 2, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new WarpBridgeDigit3Case3(GlueFactory.Create(WarpBridge, 3, u, op, msr: true, msd: true),
                                                                     GlueFactory.Create(SecondWarp, 3, u, op, msr: true, msd: true)).Tiles);
                            break;

                        default: continue;
                    }
                }
            }
        }

        private void CreateSecondWarp(IEnumerable<string> digits)
        {
            foreach (var u in digits)
            {
                foreach (var op in new[] { true, false })
                {
                    switch (u.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new SecondWarpDigit1(GlueFactory.Create(SecondWarp, 1, u, op),
                                                                GlueFactory.Create(SecondWarp, 1, u, op),
                                                                GlueFactory.Create(PostWarp,   1, u, op)).Tiles);


                            tiles.AddRange(new SecondWarpDigit2(GlueFactory.Create(SecondWarp, 2, u, op),
                                                                GlueFactory.Create(SecondWarp, 2, u, op),
                                                                GlueFactory.Create(PostWarp,   2, u, op)).Tiles);


                            tiles.AddRange(new SecondWarpDigit3(GlueFactory.Create(SecondWarp, 3, u, op),
                                                                GlueFactory.Create(SecondWarp, 3, u, op),
                                                                GlueFactory.Create(PostWarp,   3, u, op)).Tiles);
                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new SecondWarpDigit2Case2(GlueFactory.Create(SecondWarp, 2, u, op, msr: true, msd: true),
                                                                     GlueFactory.Create(SecondWarp, 2, u, op, msr: true, msd: true),
                                                                     GlueFactory.Create(PostWarp,   2, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new SecondWarpDigit3Case3(GlueFactory.Create(SecondWarp, 3, u, op, msr: true, msd: true),
                                                                     GlueFactory.Create(SecondWarp, 3, u, op, msr: true, msd: true),
                                                                     GlueFactory.Create(PostWarp,   3, u, op, msr: true, msd: true)).Tiles);
                            break;

                        default: continue;;
                    }
                }
            }
        }

        private void CreatePostWarp(IEnumerable<string> digits)
        {
            foreach (var u in digits)
            {
                foreach (var op in new[] { true, false })
                {
                    switch (u.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new PostWarpDigit1(GlueFactory.Create(PostWarp,     1, u, op),
                                                              GlueFactory.Create(CounterWrite, 1, u, op)).Tiles);
                             
                            tiles.AddRange(new PostWarpDigit2(GlueFactory.Create(PostWarp,     2, u, op),
                                                              GlueFactory.Create(CounterWrite, 2, u, op)).Tiles);

                            tiles.AddRange(new PostWarpDigit3(GlueFactory.Create(PostWarp,     3, u, op),
                                                              GlueFactory.Create(CounterWrite, 3, u, op)).Tiles);
                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new PostWarpDigit1Case2(GlueFactory.Create(PostWarp,     1, u, op),
                                                                   GlueFactory.Create(CounterWrite, 1, u, op, msr: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new PostWarpDigit1Case1(GlueFactory.Create(PostWarp,     1, u, op),
                                                                   GlueFactory.Create(CounterWrite, 1, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new PostWarpDigit2Case2(GlueFactory.Create(PostWarp,     2, u, op),
                                                                   GlueFactory.Create(CounterWrite, 2, u, op, msr: true, msd: true)).Tiles);
                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new PostWarpDigit3Case3(GlueFactory.Create(PostWarp,     3, u, op),
                                                                   GlueFactory.Create(CounterWrite, 3, u, op, msr: true, msd: true)).Tiles);
                            break; 

                        default: continue;
                    }
                }
            }
        }

        private void CreateDigitTops()
        {
            foreach (var op in new []{true, false})
            {
                tiles.AddRange(new DigitTop(L,
                                            GlueFactory.Create(DigitTop,   1, op),
                                            GlueFactory.Create(ReturnPath, 1, op)).Tiles);

                tiles.AddRange(new DigitTop(L, 
                                            GlueFactory.Create(DigitTop,   2, op), 
                                            GlueFactory.Create(ReturnPath, 2, op)).Tiles);

                tiles.AddRange(new DigitTop(L,
                                            GlueFactory.Create(DigitTop,   3, op),
                                            GlueFactory.Create(ReturnPath, 3, op)).Tiles);


                switch (digitsInMSR)
                {
                    case 1:
                        tiles.AddRange(new DigitTopDigit1Case1(L,
                                                               GlueFactory.Create(DigitTop,   1, op, msr: true, msd: true),
                                                               GlueFactory.Create(ReturnPath, 1, op, msr: true, msd: true)).Tiles);
                        break;

                    case 2:
                        tiles.AddRange(new DigitTopDigit1Case2(L, 
                                                               GlueFactory.Create(DigitTop,   1, op,  msr: true), 
                                                               GlueFactory.Create(ReturnPath, 1, op,  msr: true)).Tiles);

                        tiles.AddRange(new DigitTopDigit2Case2(L, 
                                                               GlueFactory.Create(DigitTop,   2, op, msr: true, msd: true),
                                                               GlueFactory.Create(ReturnPath, 2, op, msr: true, msd: true)).Tiles);
                        break;
                    case 3:
                        tiles.AddRange(new DigitTop(L,
                                                    GlueFactory.Create(DigitTop,   3, op, msr: true, msd: true),
                                                    GlueFactory.Create(ReturnPath, 3, op, msr: true, msd: true)).Tiles);
                        break;
                }

            }
        
        }

        private void CreateReturnPaths()
        {
            foreach (var op in new[] { true, false })
            {
                tiles.AddRange(new ReturnPathDigit1(L,
                                                    GlueFactory.Create(ReturnPath, 1, op),
                                                    GlueFactory.Create(NextRead,   1, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit1Case2(L,
                                                         GlueFactory.Create(ReturnPath, 1, op, msr: true),
                                                         GlueFactory.Create(NextRead,   1, op, msr: true)).Tiles);

                tiles.AddRange(new ReturnPathDigit1Case1(L,
                                                         GlueFactory.Create(ReturnPath, 1, op, msr: true, msd: true),
                                                         GlueFactory.Create(NextRead,   1, op, msr: true, msd: true)).Tiles);

                tiles.AddRange(new ReturnPathDigit2(L,
                                                    GlueFactory.Create(ReturnPath, 2, op),
                                                    GlueFactory.Create(NextRead,   2, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit2Case2(L,
                                                         GlueFactory.Create(ReturnPath, 2, op),
                                                         GlueFactory.Create(NextRead,   2, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit3(L,
                                                    GlueFactory.Create(ReturnPath, 3, op),
                                                    GlueFactory.Create(NextRead,   3, op)).Tiles);

                tiles.AddRange(new ReturnPathDigit3Case3(L,
                                                         GlueFactory.Create(ReturnPath, 3, op, msr: true, msd: true),
                                                         GlueFactory.Create(NextRead,   3, op, msr: true, msd: true)).Tiles);
            }
        }

        private void CreateNextRead()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new NextReadDigit1(L,
                                                  GlueFactory.Create(NextRead,    1, op), 
                                                  GlueFactory.Create(CounterRead, 2, op)).Tiles);

                tiles.AddRange(new NextReadDigit1Case2(L,
                                                       GlueFactory.Create(NextRead,    1, op, msr: true),
                                                       GlueFactory.Create(CounterRead, 2, op)).Tiles);

                tiles.AddRange(new NextReadDigit1Case1(L,
                                                       GlueFactory.Create(NextRead,     1, op, msr: true, msd: true),
                                                       GlueFactory.Create(CrossNextRow,    op)).Tiles);

                tiles.AddRange(new NextReadDigit2(L,
                                                  GlueFactory.Create(NextRead,    2, op),
                                                  GlueFactory.Create(CounterRead, 3, op)).Tiles);

                tiles.AddRange(new NextReadDigit2Case2(L,
                                                       GlueFactory.Create(NextRead,     2, op, msr: true, msd: true),
                                                       GlueFactory.Create(CrossNextRow,    op)).Tiles);

                tiles.AddRange(new NextReadDigit3(L,
                                                  GlueFactory.Create(NextRead,    3, op),
                                                  GlueFactory.Create(CounterRead, 1, op)).Tiles);

                tiles.AddRange(new NextReadDigit3Case3(L,
                                                       GlueFactory.Create(NextRead,    3, op, msr: true, msd: true),
                                                       GlueFactory.Create(CrossNextRow,   op)).Tiles);
            }
        }


        private void CreateCrossNextRow()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new CrossNextRow(construction.d,
                                                GlueFactory.Create(CrossNextRow, op), 
                                                GlueFactory.Create(CounterRead,  1, string.Empty, op)).Tiles);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        private IEnumerable<Tile> CreateWriters(string bits)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new BinaryWriter(bits, true,  i, digitsInMSR).Tiles);
                results.AddRange(new BinaryWriter(bits, false, i, digitsInMSR).Tiles);
            }

            return results;
        }




    }
}
