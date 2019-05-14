namespace WarpingCounter.Gadgets
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    public class ReaderFactory
    {

        private const int Digits = 3;

        private readonly int L;

        private readonly int M;

        private readonly int logM;
        
        public readonly List<string> DigitsWithLengthL;

        public readonly List<BinaryReader> Readers;

        public readonly HashSet<string> DigitsThatCanBeRead;


        public ReaderFactory(int L, int logM, int M)
        {
            this.L     = L;
            this.logM = logM;
            this.M     = M;

            DigitsThatCanBeRead = new HashSet<string>();
            DigitsWithLengthL   = new List<string>();
            Readers             = CreateCounterReaders();
        }


        private List<BinaryReader> CreateCounterReaders()
        {
            var results = new List<BinaryReader>();

            // for i = 0,...,M
            for (var i = 0; i < M; i++)
            {
                var value = Convert.ToString(i, 2)
                                   .PadLeft(logM, '0');

                DigitsWithLengthL.Add($"{value}00");
                DigitsWithLengthL.Add($"{value}01");
                DigitsWithLengthL.Add($"{value}11");
            }

            // for u ∈ {0, 1}^L
            foreach (var U in DigitsWithLengthL)
            {
                // for i = 0,...,L
                for (var i = 0; i <= U.Length; i++)
                {
                    // take all substrings, from the end  
                    var substring = U.Substring(U.Length - i);
                    DigitsThatCanBeRead.Add(substring);
                }
            }

            Console.WriteLine($"Unique: {DigitsThatCanBeRead.Count}");

            // is this the equivalent of: for i = 0, U ∈ {0, 1}^i ? 
            foreach (var bitsRead in DigitsThatCanBeRead.OrderBy(s => s.Length)
                                                        .ThenBy(s => s))
            {
                for (var i = 1; i <= Digits; i++)
                {
                    results.Add(new BinaryReader(bitsRead, true,  i, L, M));
                    results.Add(new BinaryReader(bitsRead, false, i, L, M));
                }
            }

            return results;
        }

    }

}
