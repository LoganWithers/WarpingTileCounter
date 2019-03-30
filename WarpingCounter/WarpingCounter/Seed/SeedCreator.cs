namespace WarpingCounter.Seed
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Models;

    using Gadgets;

    using Regions;

    public class SeedCreator
    {

        public readonly ConstructionDetails Construction;

        public List<Tile> Tiles { get; }

        private string ToBinary(string key) => Construction.BinaryDigitEncodings[key];


        public SeedCreator(int baseM, string initialValueBase10)
        {
            Tiles        = new List<Tile>();
            Construction = new ConstructionDetails(initialValueBase10, baseM);
            CreateTilesForInitialValue();
        }


        private void CreateTilesForInitialValue()
        {
            List<IEnumerable<string>> regions = Construction.SplitIntoDigitRegions().ToList();
            List<string> msr = regions[0].ToList();
            var isMsrLeastSignificant = regions.Count == 1;

            switch (Construction.DigitsInMSR)
            {
                case 1:
                    CreateMsr(msr[0]);
                    break;
                case 2:
                    CreateMsr(msr[0], msr[1]);
                    break;
                case 3:
                    CreateMsr(msr[0], msr[1], msr[2], isMsrLeastSignificant);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Construction.DigitsInMSR));
            }


            for (var regionIndex = 1; regionIndex < regions.Count; regionIndex++)
            {
                List<string> region = regions[regionIndex].ToList();

                CreateStandardRegion(region[0], region[1], region[2], regionIndex, regionIndex + 1 == regions.Count);
            }
        }


        /// <summary>
        /// Creates the MSR when it has only 1 digit encoded in it. 
        /// </summary>
        /// <param name="digit1BaseM">The most significant digit in a region (the only one).</param>
        private void CreateMsr(string digit1BaseM)
        {
            var digit1 = $"{ToBinary(digit1BaseM)}11";
            Console.WriteLine($"Region 0 (MSR):\n    D1: {digit1}\n\n");

            var region = new OneDigitRegion(Construction.ActualBitsPerDigit, digit1);
            Tiles.AddRange(region.Tiles);
        }


        /// <param name="digit2BaseM">The most significant digit.</param>
        /// <param name="digit1BaseM">The second-most significant digit.</param>
        private void CreateMsr(string digit2BaseM, string digit1BaseM)
        {
            var digit2 = $"{ToBinary(digit2BaseM)}11";
            var digit1 = $"{ToBinary(digit1BaseM)}01";
            Console.WriteLine($"Region 0 (MSR):\n    D2: {digit2}\n    D1: {digit1}\n\n");


            var region = new TwoDigitRegion(Construction.ActualBitsPerDigit, (digit2, digit1));
            Tiles.AddRange(region.Tiles);
        }


        /// <param name="digit3BaseM">The most-significant digit</param>
        /// <param name="digit2BaseM">The second-most significant digit</param>
        /// <param name="digit1BaseM">The third-most significant digit</param>
        /// <param name="isLeastSignificant"></param>
        private void CreateMsr(string digit3BaseM,
                               string digit2BaseM,
                               string digit1BaseM,
                               bool isLeastSignificant)
        {
            var digit3 = $"{ToBinary(digit3BaseM)}11";
            var digit2 = $"{ToBinary(digit2BaseM)}00";
            var digit1 = $"{ToBinary(digit1BaseM)}00";
            Console.WriteLine($"Region 0 (MSR):\n    D3: {digit3}\n    D2: {digit2}\n    D1: {digit1}\n\n");

            var region = new ThreeDigitRegion(Construction.ActualBitsPerDigit,
                                                    (digit3, digit2, digit1),
                                                    0,
                                                    isLeastSignificant);

            Tiles.AddRange(region.Tiles);
        }



        private void CreateStandardRegion(string digit3BaseM,
                                          string digit2BaseM,
                                          string digit1BaseM,
                                          int regionIndex,
                                          bool isLeastSignificant)
        {
            var digit3 = $"{ToBinary(digit3BaseM)}00";
            var digit2 = $"{ToBinary(digit2BaseM)}00";
            var digit1 = $"{ToBinary(digit1BaseM)}00";
            Console.WriteLine($"Region {regionIndex}:\n    D3: {digit3}\n    D2: {digit2}\n    D1: {digit1}\n\n");

            var region = new ThreeDigitRegion(Construction.ActualBitsPerDigit,
                                              (digit3, digit2, digit1),
                                              regionIndex,
                                              isLeastSignificant);
            Tiles.AddRange(region.Tiles);
        }




    }

}
