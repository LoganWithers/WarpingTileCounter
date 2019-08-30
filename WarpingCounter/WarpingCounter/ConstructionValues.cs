namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;

    public class ConstructionValues
    {

        private const double DigitsPerRegion = 3;

        private const int BitsUsedAsFlags = 2;

        public readonly Dictionary<string, string> BinaryDigitEncodings;

        public readonly List<string> EncodedDigits;

        public readonly int d;

        private readonly BigInteger haltingValueBase10;

        private readonly List<string> haltingValueBaseM;

        private readonly BigInteger initialValueBase10;

        private readonly List<string> initialValueBaseM;

        private readonly double power;

        public ConstructionValues(string initialValueBase10, int baseM, int d)
        {
            this.initialValueBase10 = BigInteger.Parse(initialValueBase10);
            BaseM                   = baseM;
            power                   = Math.Ceiling(BigInteger.Log(this.initialValueBase10, BaseM));
            haltingValueBase10      = BigInteger.Pow(BaseM, Convert.ToInt32(power));
            BitsRequiredForBaseM    = Convert.ToString(BaseM - 1, 2).Length;
            this.d = d;

            var leadingZeroes = BaseM.ToString().Length;
            var digits = (double) d;

            var remainder = digits % DigitsPerRegion;

            var remainderDigits = (int) remainder;
            var quotient        = (int) Math.Floor(digits / DigitsPerRegion);

            DigitRegions = IsZero(remainder) ? quotient : quotient + 1;
            DigitsInMSR  = remainderDigits == 0 ? 3 : remainderDigits;

            List<string> ConvertToBaseMWithLeadingZeroes(BigInteger value, int m)
            {
                var converted = value.ToBase(m);

                if (converted.Count < d)
                {
                    var missing = d - converted.Count;

                    converted.InsertRange(0, Enumerable.Repeat("0", missing));
                }

                return converted.Select(s => s.PadLeft(leadingZeroes, '0')).ToList();
            }

            
            initialValueBaseM = ConvertToBaseMWithLeadingZeroes(this.initialValueBase10, BaseM);
            haltingValueBaseM = ConvertToBaseMWithLeadingZeroes(haltingValueBase10,      BaseM);

            BinaryDigitEncodings = new Dictionary<string, string>();

            foreach (var digit in initialValueBaseM)
            {
                BinaryDigitEncodings[digit] = Convert.ToString(Convert.ToInt32(digit), 2)
                                                     .PadLeft(BitsRequiredForBaseM, '0');
            }

            EncodedDigits = new List<string>();

            void AddWithLeadingZeroes(string value) => EncodedDigits.Add(value.PadLeft(L, '0'));

            for (var i = 0; i < baseM; i++)
            {
                var value = Convert.ToString(i, 2);
                AddWithLeadingZeroes($"{value}01");
                AddWithLeadingZeroes($"{value}00");
                AddWithLeadingZeroes($"{value}11");
            }

        }


        public int BaseM { get; }


        public int DigitRegions { get; }


        public int DigitsInMSR { get; }


        public int BitsRequiredForBaseM { get; }

        /// <summary>
        /// The number of bits in a single digit, equal to: Ceil( Log2(M) ) + 2 
        /// </summary>
        public int L => BitsRequiredForBaseM + BitsUsedAsFlags;


        /// <summary>
        ///   Determines whether the specified floating point value is zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is zero; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsZero(double value) => value < double.Epsilon;


        private void Summarize()
        {
            Console.WriteLine("Decimal:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    Start: {initialValueBase10}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    Halt:  {haltingValueBase10}");
            Console.ResetColor();
            var length = initialValueBaseM.Max(s => s.Length);

            Console.WriteLine($"B{BaseM}:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    Start: {string.Join(" ", initialValueBaseM.Select(digit => digit.PadLeft(length, '0')))}");

            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    Halt:  {string.Join(" ", haltingValueBaseM.Select(digit => digit.PadLeft(length, '0')))}");
            Console.ResetColor();
            Console.WriteLine();
        }


        public IEnumerable<IEnumerable<string>> SplitIntoDigitRegions()
        {
            if (initialValueBaseM.Count < 3)
            {
                return new[] {initialValueBaseM};
            }

            switch (initialValueBaseM.Count % 3)
            {
                // Takes the two most significant regions, removes them, groups the other 
                // regions into groups of three, and re-inserts the two regions 
                // back into the beginning of the list
                case 2:
                {
                    var first  = initialValueBaseM[0];
                    var second = initialValueBaseM[1];
                    initialValueBaseM.RemoveAt(0);
                    initialValueBaseM.RemoveAt(0);

                    List<IEnumerable<string>> chunks = initialValueBaseM.SplitEvery(3)
                                                                        .ToList();

                    chunks.Insert(0, new[] {first, second});

                    return chunks;
                }

                // Takes the most significant regions, removes it, groups the other 
                // regions into groups of three, and re-inserts the most significant region 
                // back into the beginning of the list
                case 1:
                {
                    var first = initialValueBaseM[0];
                    initialValueBaseM.RemoveAt(0);

                    List<IEnumerable<string>> chunks = initialValueBaseM.SplitEvery(3)
                                                                        .ToList();

                    chunks.Insert(0, new[] {first});

                    return chunks;
                }

                default:
                    return initialValueBaseM.SplitEvery(3);
            }
        }


        public override string ToString() => string.Join(" ", initialValueBaseM.Select(d => $"{d}"));

    }

}
