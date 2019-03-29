namespace WarpingCounter.Gadgets
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    public class ReaderFactory
    {
        public readonly HashSet<string> UniqueDigits;
        public readonly List<BinaryReader> Readers;

        private const int Digits = 3;

        private readonly int baseOfEncodedDigits;
        private readonly int bitsRequiredForBaseM;
        private readonly int actualBitsPerEncodedDigit;

        private readonly List<string> possibilities;

        public ReaderFactory(int actualBitsPerEncodedDigit, int bitsRequiredForBaseM, int baseOfEncodedDigits)
        {
            this.actualBitsPerEncodedDigit = actualBitsPerEncodedDigit;
            this.bitsRequiredForBaseM = bitsRequiredForBaseM;
            this.baseOfEncodedDigits = baseOfEncodedDigits;
   
            UniqueDigits = new HashSet<string>();
            possibilities = new List<string>();
            Readers = CreateCounterRead();
        }


        private List<BinaryReader> CreateCounterRead()
        {
            var results = new List<BinaryReader>();

            for (var i = 0; i < baseOfEncodedDigits; i++)
            {
                var value = Convert.ToString(i, 2).PadLeft(bitsRequiredForBaseM, '0');
                possibilities.Add(value);
                possibilities.Add($"{value}00");
                possibilities.Add($"{value}01");
                possibilities.Add($"{value}11");
            }

            foreach (var possibility in possibilities)
            {
                for (var i = 0; i <= possibility.Length; i++)
                {
                    var data = possibility.Substring(0, i);
                    UniqueDigits.Add(data);
                }

                // since "01" reversed is "10", which indicates invalid digit/region indicators,
                // skip this.
                if (possibility.StartsWith("01"))
                {
                    continue;
                }

                var reversed = StringUtils.Reverse(possibility);
                
                for (var i = 0; i <= reversed.Length; i++)
                {
                    var data = reversed.Substring(0, i);
                    UniqueDigits.Add(data);
                }
            }

            foreach (var key in UniqueDigits.OrderBy(s => s.Length).ThenBy(s => s))
            {
                for (var i = 1;  i <= Digits; i++)
                {
                     results.Add(new BinaryReader(key, true,  i, actualBitsPerEncodedDigit, baseOfEncodedDigits));
                     results.Add(new BinaryReader(key, false, i, actualBitsPerEncodedDigit, baseOfEncodedDigits));
                }  
            }

            return results;
        }

    }
    
}
