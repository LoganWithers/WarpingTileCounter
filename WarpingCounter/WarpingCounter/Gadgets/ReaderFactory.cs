namespace WarpingCounter.Gadgets
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    public class ReaderFactory
    {

        private const int Digits = 3;

        private readonly int actualBitsPerEncodedDigit;

        private readonly int baseOfEncodedDigits;

        private readonly int bitsRequiredForBaseM;

        private readonly int digitsInMSR;

        private readonly List<string> possibilities;

        public readonly List<BinaryReader> Readers;

        public readonly HashSet<string> UniqueDigits;


        public ReaderFactory(int actualBitsPerEncodedDigit, int bitsRequiredForBaseM, int baseOfEncodedDigits, int digitsInMSR)
        {
            this.actualBitsPerEncodedDigit = actualBitsPerEncodedDigit;
            this.bitsRequiredForBaseM      = bitsRequiredForBaseM;
            this.baseOfEncodedDigits       = baseOfEncodedDigits;
            this.digitsInMSR               = digitsInMSR;

            UniqueDigits  = new HashSet<string>();
            possibilities = new List<string>();
            Readers       = CreateCounterReaders();
        }


        private List<BinaryReader> CreateCounterReaders()
        {
            var results = new List<BinaryReader>();

            for (var i = 0; i < baseOfEncodedDigits; i++)
            {
                var value = Convert.ToString(i, 2)
                                   .PadLeft(bitsRequiredForBaseM, '0');

                possibilities.Add(value);
                possibilities.Add($"{value}00");
                possibilities.Add($"{value}11");
                possibilities.Add($"{value}01");
            }

            foreach (var possibility in possibilities)
            {
                for (var i = 0; i <= possibility.Length; i++)
                {
                    var substring = possibility.Substring(possibility.Length - i);
                    UniqueDigits.Add(substring);
                }
            }

            Console.WriteLine($"Unique: {UniqueDigits.Count}");

            foreach (var key in UniqueDigits.OrderBy(s => s.Length)
                                            .ThenBy(s => s))
            {
                for (var i = 1; i <= Digits; i++)
                {
                    results.Add(new BinaryReader(key, true,  i, actualBitsPerEncodedDigit, baseOfEncodedDigits));
                    results.Add(new BinaryReader(key, false, i, actualBitsPerEncodedDigit, baseOfEncodedDigits));
                }
            }

            return results;
        }

    }

}
