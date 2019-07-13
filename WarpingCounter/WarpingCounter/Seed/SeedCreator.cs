namespace WarpingCounter.Seed
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Models;

    using Gadgets;

    using InitialValue;


    /// <summary>
    /// Takes the construction
    /// </summary>
    public class SeedCreator
    {

        private readonly ConstructionValues construction;

        
        public SeedCreator(ConstructionValues construction)
        {
            Tiles        = new List<Tile>();
            this.construction = construction;

            CreateTilesForInitialValue();
        }


        public List<Tile> Tiles { get; }


        private string ToBinary(string key) => construction.BinaryDigitEncodings[key];

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">DigitsInMSR</exception>
        private void CreateTilesForInitialValue()
        {
            List<IEnumerable<string>> regions = construction.SplitIntoDigitRegions()
                                                            .ToList();
            regions.Reverse();

            for (var i = 0; i < regions.Count; i++)
            {
                List<string> region = regions[i].ToList();
                if (i == regions.Count - 1)
                {
                    switch (construction.DigitsInMSR)
                    {
                        case 1:
                            CreateMsr(region[0], i);

                            break;
                        case 2:
                            CreateMsr(region[0], region[1], i);

                            break;
                        case 3:
                            CreateMsr(region[0], region[1], region[2], i);

                            break;
                        default:

                            throw new ArgumentOutOfRangeException(nameof(construction.DigitsInMSR));
                    }

                } else
                {
                    CreateStandardRegion(region[0], region[1], region[2], i);
                }

                
            }
        }


        /// <summary>
        ///   Creates the MSR when it has only 1 digit encoded in it.
        /// </summary>
        /// <param name="digit1BaseM">The most significant digit in a region (the only one).</param>
        private void CreateMsr(string digit1BaseM, int regionIndex)
        {
            var digit1 = $"{ToBinary(digit1BaseM)}11";
            Console.WriteLine($"Region 0 (MSR):\n    D1: {digit1}\n\n");

            var region = new Case1DigitRegion(digit1, regionIndex, construction.L);
            Tiles.AddRange(region.Tiles);
        }


        /// <param name="digit2BaseM">The most significant digit.</param>
        /// <param name="digit1BaseM">The second-most significant digit.</param>
        private void CreateMsr(string digit2BaseM, string digit1BaseM, int regionIndex)
        {
            var digit2 = $"{ToBinary(digit2BaseM)}11";
            var digit1 = $"{ToBinary(digit1BaseM)}01";
            Console.WriteLine($"Region 0 (MSR):\n    D2: {digit2}\n    D1: {digit1}\n\n");

           var region = new Case2DigitRegion((digit1, digit2), regionIndex, digit1.Length);
           Tiles.AddRange(region.Tiles);
        }


        /// <param name="digit3BaseM">The most-significant digit</param>
        /// <param name="digit2BaseM">The second-most significant digit</param>
        /// <param name="digit1BaseM">The third-most significant digit</param>
        /// <param name="regionIndex"></param>
        private void CreateMsr(string digit3BaseM,
                               string digit2BaseM,
                               string digit1BaseM,
                               int    regionIndex)
        {
            var digit3 = $"{ToBinary(digit3BaseM)}11";
            var digit2 = $"{ToBinary(digit2BaseM)}00";
            var digit1 = $"{ToBinary(digit1BaseM)}00";
            Console.WriteLine($"MSR:\n    D3: {digit3}\n    D2: {digit2}\n    D1: {digit1}\n\n");
            var region = new Case3DigitRegion((digit1, digit2, digit3),
                                              regionIndex,
                                              construction.L);

            Tiles.AddRange(region.Tiles);
        }


        private void CreateStandardRegion(string digit3BaseM,
                                          string digit2BaseM,
                                          string digit1BaseM,
                                          int    regionIndex)
        {
            var digit3 = $"{ToBinary(digit3BaseM)}00";
            var digit2 = $"{ToBinary(digit2BaseM)}00";
            var digit1 = $"{ToBinary(digit1BaseM)}00";
            Console.WriteLine($"Region {regionIndex}:\n    D3: {digit3}\n    D2: {digit2}\n    D1: {digit1}\n\n");
            var region = new GeneralDigitRegion((digit1, digit2, digit3), regionIndex, digit1.Length);

            Tiles.AddRange(region.Tiles);
        }

    }

}
