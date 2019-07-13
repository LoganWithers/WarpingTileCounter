namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;

    public class BinaryStringPermutations
    {

        private readonly int M;

        private readonly int L;

        private readonly int logM;

        public readonly ISet<string> Permutations = new SortedSet<string>();

        private readonly List<string> DigitsWithLengthL = new List<string>();
        public BinaryStringPermutations(int L, int logM, int M)
        {
            this.L    = L;
            this.logM = logM;
            this.M    = M;

            Create();
        }


        private void Create()
        {
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
                    Permutations.Add(substring);
                }
            }

        }
    }
}
