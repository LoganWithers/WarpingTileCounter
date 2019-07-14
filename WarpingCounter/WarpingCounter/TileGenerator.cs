namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

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

    using InitialValue;

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

        private readonly string name; 
        public TileGenerator(string name, int m, string initialValueB10)
        {
            M            = m;
            construction = new ConstructionValues(initialValueB10, m);
            digitsInMSR  = construction.DigitsInMSR;
            this.name    = $"thin_rectangle_case_{digitsInMSR}_{name}";
            L            = construction.L;
            logM         = construction.BitsRequiredForBaseM;
            regions      = construction.DigitRegions;

            Console.WriteLine($"Bits Per Counter Digit: {logM}");
            Console.WriteLine($"Actual Bits Per Digit:  {L}");
        }


        public bool IsStartingValueTooSmall() => construction.DigitRegions < 2;

        public (string name, List<Tile> tileset) Generate()
        {
            tiles.Add(new Tile("seed") {North = new Glue($"{CounterWrite} {1} {Seed} 0 {1} ")});
            //tiles.Add(new Tile("seed") {North = Bind(RoofUnit, false)});
            var filler = new Tile("Filler") {North = new Glue("Filler"), South = new Glue("Filler")};
            tiles.Add(filler);
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
            CreateRoofUnit();
            var before = tiles.Count;
            
            var after = tiles.DistinctBy(t => t.Name)
                             .ToList();

            Console.Write($"Found {before - after.Count} duplicate tiles");

            return (name.Replace(" ", "_")
                        .ToLower(), after);
        }


        private void CreateRoofUnit()
        {
            tiles.AddRange(new RoofUnit(construction.DigitsInMSR, construction.d, L, Bind(RoofUnit, Op.Halt)).Tiles);
        }


        private void CreateSeed()
        {
            tiles.AddRange(new InitialValueGenerator(construction).Tiles);
        }

        private void CreateCounterWrite(IReadOnlyCollection<string> digits)
        {
            for (var i = 1; i <= Digits; i++)
            {
                var digitsBeforeMSB = digits.Where(digit => 1 <= digit.Length && digit.Length <= L - 1)
                                            .OrderBy(s => s.Length)
                                            .ThenBy(s => s)
                                            .ToList();

                foreach (var op in new[] {Op.Increment, Op.Copy, Op.Halt})
                {
                    foreach (var U in digitsBeforeMSB)
                    {
                        tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, op, msr: false, msd: false, bits: $"{U}0"),
                                                         Bind(CounterWrite, i, op, msr: false, msd: false, bits: $"{U}0"),
                                                         Bind(CounterWrite, i, op, msr: false, msd: false, bits: U)).Tiles);

                        tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, op, msr: false, msd: false, bits: $"{U}1"),
                                                         Bind(CounterWrite, i, op, msr: false, msd: false, bits: $"{U}1"),
                                                         Bind(CounterWrite, i, op, msr: false, msd: false, bits: U)).Tiles);

                        tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, op, msr: true, msd: false, bits: $"{U}0"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: false, bits: $"{U}0"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: false, bits: U)).Tiles);

                        tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, op, msr: true, msd: false, bits: $"{U}1"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: false, bits: $"{U}1"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: false, bits: U)).Tiles);

                        tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, op, msr: true, msd: true, bits: $"{U}0"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: true, bits: $"{U}0"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: true, bits: U)).Tiles);

                        tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, op, msr: true, msd: true, bits: $"{U}1"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: true, bits: $"{U}1"),
                                                         Bind(CounterWrite, i, op, msr: true, msd: true, bits: U)).Tiles);
                    }
                }

                foreach (var op in new[] { Op.Increment, Op.Copy})
                {
                    tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, op, msr: false, msd: false, bits: "0"),
                                                     Bind(CounterWrite, i, op, msr: false, msd: false, bits: "0"),
                                                     Bind(DigitTop,     i, op, msr: false, msd: false)).Tiles);

                    tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, op, msr: false, msd: false, bits: "1"),
                                                     Bind(CounterWrite, i, op, msr: false, msd: false, bits: "1"),
                                                     Bind(DigitTop,     i, op, msr: false, msd: false)).Tiles);

                    tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, op, msr: true, msd: false, bits: "0"),
                                                     Bind(CounterWrite, i, op, msr: true, msd: false, bits: "0"),
                                                     Bind(DigitTop,     i, op, msr: true, msd: false)).Tiles);

                    tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, op, msr: true, msd: false, bits: "1"),
                                                     Bind(CounterWrite, i, op, msr: true, msd: false, bits: "1"),
                                                     Bind(DigitTop,     i, op, msr: true, msd: false)).Tiles);

                    tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, op, msr: true, msd: true, bits: "0"),
                                                     Bind(CounterWrite, i, op, msr: true, msd: true, bits: "0"),
                                                     Bind(DigitTop,     i, op, msr: true, msd: true)).Tiles);

                    tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, op, msr: true, msd: true, bits: "1"),
                                                     Bind(CounterWrite, i, op, msr: true, msd: true, bits: "1"),
                                                     Bind(DigitTop,     i, op, msr: true, msd: true)).Tiles);

                }

                tiles.AddRange(new CounterWrite0(Name(CounterWrite, i, Op.Halt, msr: true, msd: true, bits: "0"),
                                                 Bind(CounterWrite, i, Op.Halt, msr: true, msd: true, bits: "0"),
                                                 Bind(RoofUnit, Op.Halt)).Tiles);

                tiles.AddRange(new CounterWrite1(Name(CounterWrite, i, Op.Halt, msr: true, msd: true, bits: "1"),
                                                 Bind(CounterWrite, i, Op.Halt, msr: true, msd: true, bits: "1"),
                                                 Bind(RoofUnit, Op.Halt)).Tiles);
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
                return Bind(PreWarp, i, Op.Increment, msr: false, msd: false, bits: ConvertToBinary(ConvertToDecimal(valueBits) + 1) + indicatorBits);
            }

            if (indicatorBits == "11")
            {
                return Bind(PreWarp, i, Op.Halt, msr: false, msd: false, $"{zeroes}11"); // TODO: change op: false to op: "halt"
            }

            return Bind(PreWarp, i, Op.Increment, msr: false, msd: false, bits: zeroes + indicatorBits);
        }


        private void CreateCounterRead(IReadOnlyCollection<string> digits)
        {
            for (var i = 1; i <= Digits; i++)
            {
                var digitsBeforeMSB = digits.Where(digit => digit.Length <= L - 2)
                                            .OrderBy(s => s.Length)
                                            .ThenBy(s => s)
                                            .ToList();

                foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
                {
                    foreach (var U in digitsBeforeMSB)
                    {
                        tiles.AddRange(new CounterRead(Name(CounterRead, i, op, msr: false, msd: false, bits: U),
                                                       Bind(CounterRead, i, op, msr: false, msd: false, bits: U),
                                                       Bind(CounterRead, i, op, msr: false, msd: false, bits: $"1{U}"),
                                                       Bind(CounterRead, i, op, msr: false, msd: false, bits: $"0{U}")).Tiles);
                    }
                }

                var digitsForMSB = digits.Where(digit => digit.Length == L - 1)
                                         .OrderBy(s => s.Length)
                                         .ThenBy(s => s)
                                         .ToList();

                foreach (var U in digitsForMSB)
                {
                    tiles.AddRange(new CounterRead(Name(CounterRead, i, Op.Copy, msr: false, msd: false, bits: U),
                                                   Bind(CounterRead, i, Op.Copy, msr: false, msd: false, bits: U),
                                                   Bind(PreWarp,     i, Op.Copy, msr: false, msd: false, bits: $"1{U}"),
                                                   Bind(PreWarp,     i, Op.Copy, msr: false, msd: false, bits: $"0{U}")).Tiles);
                }

                foreach (var U in digitsForMSB)
                {
                    var (out0, out1) = ReadMostSignificantBit(U, i);

                    tiles.AddRange(new CounterRead(Name(CounterRead, i, Op.Increment, msr: false, msd: false, U),
                                                   Bind(CounterRead, i, Op.Increment, msr: false, msd: false, U),
                                                   outputOne: out1,
                                                   outputZero: out0).Tiles);
                }
            }
        }


        private void CreatePreWarp(IEnumerable<string> digits)
        {
            foreach (var U in digits)
            {
                foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new PreWarpDigit1(Name(PreWarp,   1, op, msr: false, msd: false, bits: U),
                                                             Bind(PreWarp,   1, op, msr: false, msd: false, bits: U),
                                                             Bind(FirstWarp, 1, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new PreWarpDigit2(Name(PreWarp,   2, op, msr: false, msd: false, bits: U),
                                                             Bind(PreWarp,   2, op, msr: false, msd: false, bits: U),
                                                             Bind(FirstWarp, 2, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new PreWarpDigit3(Name(PreWarp,   3, op, msr: false, msd: false, bits: U),
                                                             Bind(PreWarp,   3, op, msr: false, msd: false, bits: U),
                                                             Bind(FirstWarp, 3, op, msr: false, msd: false, bits: U)).Tiles);

                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new PreWarpDigit1Case2(Name(PreWarp,   1, op, msr: true,  msd: false, bits: U),
                                                                  Bind(PreWarp,   1, op, msr: false, msd: false, bits: U),
                                                                  Bind(FirstWarp, 1, op, msr: true,  msd: false, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new PreWarpDigit1Case1(Name(PreWarp,   1, op, msr: true,  msd: true,  bits: U),
                                                                  Bind(PreWarp,   1, op, msr: false, msd: false, bits: U),
                                                                  Bind(FirstWarp, 1, op, msr: true,  msd: true,  bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new PreWarpDigit2Case2(Name(PreWarp,   2, op, msr: true,  msd: true,  bits: U),
                                                                  Bind(PreWarp,   2, op, msr: false, msd: false, bits: U),
                                                                  Bind(FirstWarp, 2, op, msr: true,  msd: true,  bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new PreWarpDigit3Case3(Name(PreWarp,   3, op, msr: true,  msd: true,  bits: U),
                                                                  Bind(PreWarp,   3, op, msr: false, msd: false, bits: U),
                                                                  Bind(FirstWarp, 3, op, msr: true,  msd: true,  bits: U)).Tiles);
                             
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
                foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new FirstWarpDigit1(Name(FirstWarp,  1, op, msr: false, msd: false, bits: U),
                                                               Bind(FirstWarp,  1, op, msr: false, msd: false, bits: U),
                                                               Bind(FirstWarp,  1, op, msr: false, msd: false, bits: U),
                                                               Bind(WarpBridge, 1, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new FirstWarpDigit2(Name(FirstWarp,  2, op, msr: false, msd: false, bits: U),
                                                               Bind(FirstWarp,  2, op, msr: false, msd: false, bits: U),
                                                               Bind(FirstWarp,  2, op, msr: false, msd: false, bits: U),
                                                               Bind(WarpBridge, 2, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new FirstWarpDigit3(Name(FirstWarp,  3, op, msr: false, msd: false, bits: U),
                                                               Bind(FirstWarp,  3, op, msr: false, msd: false, bits: U),
                                                               Bind(FirstWarp,  3, op, msr: false, msd: false, bits: U),
                                                               Bind(WarpBridge, 3, op, msr: false, msd: false, bits: U)).Tiles);

                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new FirstWarpDigit1Case2(Name(FirstWarp, 1, op, msr: true, msd: false, bits: U),
                                                                    Bind(FirstWarp, 1, op, msr: true, msd: false, bits: U),
                                                                    Bind(FirstWarp, 1, op, msr: true, msd: false, bits: U),
                                                                    Bind(PostWarp,  1, op, msr: true, msd: false, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new FirstWarpDigit1Case1(Name(FirstWarp, 1, op, msr: true, msd: true, bits: U),
                                                                    Bind(FirstWarp, 1, op, msr: true, msd: true, bits: U),
                                                                    Bind(FirstWarp, 1, op, msr: true, msd: true, bits: U),
                                                                    Bind(PostWarp,  1, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new FirstWarpDigit2Case2(Name(FirstWarp,  2, op, msr: true, msd: true, bits: U),
                                                                    Bind(FirstWarp,  2, op, msr: true, msd: true, bits: U),
                                                                    Bind(FirstWarp,  2, op, msr: true, msd: true, bits: U),
                                                                    Bind(WarpBridge, 2, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new FirstWarpDigit3Case3(Name(FirstWarp,  3, op, msr: true, msd: true, bits: U),
                                                                    Bind(FirstWarp,  3, op, msr: true, msd: true, bits: U),
                                                                    Bind(FirstWarp,  3, op, msr: true, msd: true, bits: U),
                                                                    Bind(WarpBridge, 3, op, msr: true, msd: true, bits: U)).Tiles);

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
                foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new WarpBridgeDigit1(Name(WarpBridge, 1, op, msr: false, msd: false, bits: U),
                                                                Bind(WarpBridge, 1, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 1, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new WarpBridgeDigit2(Name(WarpBridge, 2, op, msr: false, msd: false, bits: U),
                                                                Bind(WarpBridge, 2, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 2, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new WarpBridgeDigit3(Name(WarpBridge, 3, op, msr: false, msd: false, bits: U),
                                                                Bind(WarpBridge, 3, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 3, op, msr: false, msd: false, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new WarpBridgeDigit2Case2(Name(WarpBridge, 2, op, msr: true,  msd: true, bits: U),
                                                                     Bind(WarpBridge, 2, op, msr: true,  msd: true, bits: U),
                                                                     Bind(SecondWarp, 2, op, msr: true,  msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new WarpBridgeDigit3Case3(Name(WarpBridge, 3, op, msr: true,  msd: true, bits: U),
                                                                     Bind(WarpBridge, 3, op, msr: true,  msd: true, bits: U),
                                                                     Bind(SecondWarp, 3, op, msr: true,  msd: true, bits: U)).Tiles);

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
                foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new SecondWarpDigit1(Name(SecondWarp, 1, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 1, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 1, op, msr: false, msd: false, bits: U),
                                                                Bind(PostWarp,   1, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new SecondWarpDigit2(Name(SecondWarp, 2, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 2, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 2, op, msr: false, msd: false, bits: U),
                                                                Bind(PostWarp,   2, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new SecondWarpDigit3(Name(SecondWarp, 3, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 3, op, msr: false, msd: false, bits: U),
                                                                Bind(SecondWarp, 3, op, msr: false, msd: false, bits: U),
                                                                Bind(PostWarp,   3, op, msr: false, msd: false, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new SecondWarpDigit2Case2(Name(SecondWarp, 2, op, msr: true, msd: true, bits: U),
                                                                     Bind(SecondWarp, 2, op, msr: true, msd: true, bits: U),
                                                                     Bind(SecondWarp, 2, op, msr: true, msd: true, bits: U),
                                                                     Bind(PostWarp,   2, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new SecondWarpDigit3Case3(Name(SecondWarp, 3, op, msr: true, msd: true, bits: U),
                                                                     Bind(SecondWarp, 3, op, msr: true, msd: true, bits: U),
                                                                     Bind(SecondWarp, 3, op, msr: true, msd: true, bits: U),
                                                                     Bind(PostWarp,   3, op, msr: true, msd: true, bits: U)).Tiles);

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
                foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
                {
                    switch (U.GetLast(2))
                    {
                        case "00":
                            tiles.AddRange(new PostWarpDigit1(Name(PostWarp,     1, op, msr: false, msd: false, bits: U),
                                                              Bind(PostWarp,     1, op, msr: false, msd: false, bits: U),
                                                              Bind(CounterWrite, 1, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new PostWarpDigit2(Name(PostWarp,     2, op, msr: false, msd: false, bits: U),
                                                              Bind(PostWarp,     2, op, msr: false, msd: false, bits: U),
                                                              Bind(CounterWrite, 2, op, msr: false, msd: false, bits: U)).Tiles);

                            tiles.AddRange(new PostWarpDigit3(Name(PostWarp,     3, op, msr: false, msd: false, bits: U),
                                                              Bind(PostWarp,     3, op, msr: false, msd: false, bits: U),
                                                              Bind(CounterWrite, 3, op, msr: false, msd: false, bits: U)).Tiles);

                            break;

                        case "01" when digitsInMSR == 2:
                            tiles.AddRange(new PostWarpDigit1Case2(Name(PostWarp,     1, op, msr: true, msd: true, bits: U),
                                                                   Bind(PostWarp,     1, op, msr: true, msd: true, bits: U),
                                                                   Bind(CounterWrite, 1, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 1:
                            tiles.AddRange(new PostWarpDigit1Case1(Name(PostWarp,     1, op, msr: true, msd: true, bits: U),
                                                                   Bind(PostWarp,     1, op, msr: true, msd: true, bits: U),
                                                                   Bind(CounterWrite, 1, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 2:
                            tiles.AddRange(new PostWarpDigit2Case2(Name(PostWarp,     2, op, msr: true, msd: true, bits: U),
                                                                   Bind(PostWarp,     2, op, msr: true, msd: true, bits: U),
                                                                   Bind(CounterWrite, 2, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        case "11" when digitsInMSR == 3:
                            tiles.AddRange(new PostWarpDigit3Case3(Name(PostWarp,     3, op, msr: true, msd: true, bits: U),
                                                                   Bind(PostWarp,     3, op, msr: true, msd: true, bits: U),
                                                                   Bind(CounterWrite, 3, op, msr: true, msd: true, bits: U)).Tiles);

                            break;

                        default: continue;
                    }
                }
            }
        }


        private void CreateDigitTops()
        {
            foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
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
            foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
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
            foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
            {
                tiles.AddRange(new NextReadDigit1(Name(NextRead,    1, op),
                                                  L,                    
                                                  Bind(NextRead,    1, op),
                                                  Bind(CounterRead, 2, op, msr: false, msd: false, string.Empty)).Tiles);

                tiles.AddRange(new NextReadDigit1Case2(Name(NextRead,    1, op, msr: true),
                                                       L,                   
                                                       Bind(NextRead,    1, op, msr: true),
                                                       Bind(CounterRead, 2, op, msr: false, msd: false, string.Empty)).Tiles);

                tiles.AddRange(new NextReadDigit1Case1(Name(NextRead,     1, op, msr: true, msd: true),
                                                       L,
                                                       Bind(NextRead,     1, op, msr: true, msd: true),
                                                       Bind(CrossNextRow,    op)).Tiles);

                tiles.AddRange(new NextReadDigit2(Name(NextRead,    2, op),
                                                  L,                   
                                                  Bind(NextRead,    2, op),
                                                  Bind(CounterRead, 3, op, msr: false, msd: false, string.Empty)).Tiles);

                tiles.AddRange(new NextReadDigit2Case2(Name(NextRead,     2, op, msr: true, msd: true),
                                                       L,
                                                       Bind(NextRead,     2, op, msr: true, msd: true),
                                                       Bind(CrossNextRow,    op)).Tiles);

                tiles.AddRange(new NextReadDigit3(Name(NextRead,    3, op),
                                                  L,                   
                                                  Bind(NextRead,    3, op),
                                                  Bind(CounterRead, 1, op, msr: false, msd: false, string.Empty)).Tiles);

                tiles.AddRange(new NextReadDigit3Case3(Name(NextRead,     3, op, msr: true, msd: true),
                                                       Bind(NextRead,     3, op, msr: true, msd: true),
                                                       Bind(CrossNextRow,    op)).Tiles);
            }
        }


        private void CreateCrossNextRow()
        {
            foreach (var op in new[] { Op.Increment, Op.Copy, Op.Halt })
            {
                tiles.AddRange(new CrossNextRow(Name(CrossNextRow,    op),
                                                construction.d,       
                                                Bind(CrossNextRow,    op),
                                                Bind(CounterRead,  1, op, msr: false, msd: false, string.Empty)).Tiles);
            }
        }

    }

}
