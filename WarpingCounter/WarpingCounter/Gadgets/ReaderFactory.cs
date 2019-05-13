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

        public readonly List<string> DigitsWithLengthL;

        public readonly List<BinaryReader> Readers;

        public readonly HashSet<string> DigitsThatCanBeRead;


        public ReaderFactory(int actualBitsPerEncodedDigit, int bitsRequiredForBaseM, int baseOfEncodedDigits, int digitsInMSR)
        {
            this.actualBitsPerEncodedDigit = actualBitsPerEncodedDigit;
            this.bitsRequiredForBaseM      = bitsRequiredForBaseM;
            this.baseOfEncodedDigits       = baseOfEncodedDigits;
            this.digitsInMSR               = digitsInMSR;

            DigitsThatCanBeRead = new HashSet<string>();
            DigitsWithLengthL   = new List<string>();
            Readers             = CreateCounterReaders();
        }


        private List<BinaryReader> CreateCounterReaders()
        {
            var results = new List<BinaryReader>();

            for (var i = 0; i < baseOfEncodedDigits; i++)
            {
                var value = Convert.ToString(i, 2).PadLeft(bitsRequiredForBaseM, '0');

                DigitsWithLengthL.Add($"{value}00");
                DigitsWithLengthL.Add($"{value}01");
                DigitsWithLengthL.Add($"{value}11");
            }

            foreach (var lengthLDigit in DigitsWithLengthL)
            {
                for (var i = 0; i <= lengthLDigit.Length; i++)
                {
                    var substring = lengthLDigit.Substring(lengthLDigit.Length - i);
                    DigitsThatCanBeRead.Add(substring);
                }
            }

            Console.WriteLine($"Unique: {DigitsThatCanBeRead.Count}");

            foreach (var bitsRead in DigitsThatCanBeRead.OrderBy(s => s.Length)
                                                        .ThenBy(s => s))
            {
                for (var i = 1; i <= Digits; i++)
                {
                    results.Add(new BinaryReader(bitsRead, true,  i, actualBitsPerEncodedDigit, baseOfEncodedDigits));
                    results.Add(new BinaryReader(bitsRead, false, i, actualBitsPerEncodedDigit, baseOfEncodedDigits));
                }
            }

            return results;
        }

    }

}
