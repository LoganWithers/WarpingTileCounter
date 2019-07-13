namespace WarpingCounter.Gadgets
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    public class ReaderFactory
    {

        private const int Digits = 3;

        private readonly int L;

        private readonly int M;

        private readonly int logM;
        
        public readonly List<string> DigitsWithLengthL;

        public readonly List<CounterRead> Readers;

        public readonly SortedSet<string> DigitsThatCanBeRead;


        public ReaderFactory(int L, int logM, int M)
        {
            this.L     = L;
            this.logM  = logM;
            this.M     = M;

            DigitsThatCanBeRead = new SortedSet<string>();
            DigitsWithLengthL   = new List<string>();
            Readers             = CreateCounterReaders();
        }


        private List<CounterRead> CreateCounterReaders()
        {
            // for i = 0,...,M
            for (var i = 0; i < M; i++)
            {
                // Append leading 0's for digits that aren't sufficient 
                var value = Convert.ToString(i, 2).PadLeft(logM, '0');

                DigitsWithLengthL.Add($"{value}00");
                DigitsWithLengthL.Add($"{value}01");
                DigitsWithLengthL.Add($"{value}11");
            }

            // for u ∈ {0, 1}^L
            foreach (var U in DigitsWithLengthL)
            {
                // for i = 0,...,L
                for (var i = 0; i <= U.Length - 1; i++)
                {
                    // take all substrings, from the end  
                    var substring = U.Substring(U.Length - i);
                    DigitsThatCanBeRead.Add(substring);
                }
            }


            Console.WriteLine($"Unique: {DigitsThatCanBeRead.Count}");

            var results = new List<CounterRead>();
            // CounterRead 1 '0100' true
            for (var i = 1; i <= Digits; i++)
            {
                var digitsBeforeMSB = DigitsThatCanBeRead.Where(digit => digit.Length <= L - 2).OrderBy(s => s.Length).ThenBy(s => s).ToList();

                foreach (var op in new[]{true, false})
                {
                    

                    foreach (var bits in digitsBeforeMSB)
                    {
                        results.Add(new CounterRead(GlueFactory.Create(Names.CounterRead, i,     bits,   op),
                                                    GlueFactory.Create(Names.CounterRead, i, $"1{bits}", op),
                                                    GlueFactory.Create(Names.CounterRead, i, $"0{bits}", op)));
                    }
                }

                var digitsForMSB = DigitsThatCanBeRead.Where(digit => digit.Length == L - 1).OrderBy(s => s.Length).ThenBy(s => s).ToList();

                foreach (var bits in digitsForMSB)
                {
                    results.Add(new CounterRead(GlueFactory.Create(Names.CounterRead, i, bits,       op: false),
                                                GlueFactory.Create(Names.PreWarp,     i, $"1{bits}", op: false),
                                                GlueFactory.Create(Names.PreWarp,     i, $"0{bits}", op: false)));
                }

                foreach (var bits in digitsForMSB)
                {
                    var (out0, out1) = ReadMostSignificantBit(bits, i);

                    results.Add(new CounterRead(GlueFactory.Create(Names.CounterRead, i, bits, op: true),
                                                outputOne:  out1, 
                                                outputZero: out0));
                }
            }



            return results;
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
    }

}
