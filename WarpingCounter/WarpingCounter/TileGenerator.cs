namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.ReturnAndRead.NextDigit;
    using Gadgets.ReturnAndRead.NextRow;
    using Gadgets.Warping;

    using Seed;

    /// <summary>
    /// Provided the details of a counter, this creates all the gadgets needed to
    /// assemble a counter with a specific base and starting value. Namely, the
    /// following "gadgets" will be created.
    ///
    /// CounterUnits
    ///     -> Readers
    ///     -> Writers
    ///     -> Warper
    /// 
    /// </summary>
    public class TileGenerator
    {

        private const int Digits = 3;

        private readonly ConstructionValues construction;
        private readonly List<Tile> tiles = new List<Tile>();

        private readonly int digitsInMSR;
        private readonly int bitsPerDigit;
        private readonly int bitsPerCounterDigit;
        private readonly int regions;

        private readonly int baseM;

        public TileGenerator(int baseM, string initialValueB10)
        {
            this.baseM          = baseM;
            construction        = new ConstructionValues(initialValueB10, baseM);
            digitsInMSR         = construction.DigitsInMSR;
            bitsPerDigit        = construction.ActualBitsPerDigit;
            bitsPerCounterDigit = construction.BitsPerCounterDigit;
            regions             = construction.DigitRegions;

            Console.WriteLine($"Bits Per Counter Digit: {bitsPerCounterDigit}");
            Console.WriteLine($"Actual Bits Per Digit:  {bitsPerDigit}");
        }

        public bool IsStartingValueTooSmall() => construction.DigitRegions < 2;


        public List<Tile> Generate()
        {
            AddSeed();
            AddCounter();
            AddDigitTops();
            AddReturnAndRead();

            return tiles;
        }
        
        private void AddSeed()
        {
            tiles.AddRange(new SeedCreator(construction).Tiles);
        }


        private void AddCounter()
        {
            var readerFactory = new ReaderFactory(bitsPerDigit, bitsPerCounterDigit, baseM, digitsInMSR);
            tiles.AddRange(readerFactory.Readers.SelectMany(reader => reader.Tiles));
        
            // Digits with LogM + 2 length?
            List<string> fullSizeDigits = readerFactory.UniqueDigits
                                                       .Where(d => d.Length == bitsPerDigit)
                                                       .ToList();

            Console.WriteLine($"Full Digits: {fullSizeDigits.Count}");

            foreach (var binaryString in fullSizeDigits)
            {
                tiles.AddRange(CreateWarpUnits(binaryString));
                tiles.AddRange(CreateWriters(binaryString));
            }
        }

        /// <summary>
        /// Adds  
        /// </summary>
        private void AddDigitTops()
        {
            
            tiles.AddRange(new DigitTopDefault(true,  1, bitsPerDigit).Tiles);
            tiles.AddRange(new DigitTopDefault(false, 1, bitsPerDigit).Tiles);

            tiles.AddRange(new DigitTopDefault(true,  2, bitsPerDigit).Tiles);
            tiles.AddRange(new DigitTopDefault(false, 2, bitsPerDigit).Tiles);

            tiles.AddRange(new DigitTopDefault(true,  3, bitsPerDigit).Tiles);
            tiles.AddRange(new DigitTopDefault(false, 3, bitsPerDigit).Tiles);

            switch (digitsInMSR)
            {
                case 1:
                    break;
    
                case 2:
                    tiles.AddRange(new DigitTopDigit2Case2(true,  bitsPerDigit).Tiles);
                    tiles.AddRange(new DigitTopDigit2Case2(false, bitsPerDigit).Tiles);
                    tiles.AddRange(new DigitTopDigit1Case2(true,  bitsPerDigit).Tiles);
                    tiles.AddRange(new DigitTopDigit1Case2(false, bitsPerDigit).Tiles);
                    break;
    
                case 3:
                    tiles.AddRange(new DigitTopDigit3Case3(true, bitsPerDigit).Tiles);
                    tiles.AddRange(new DigitTopDigit3Case3(false, bitsPerDigit).Tiles);
                    break;
            }
        
        }

        private void AddReturnAndRead()
        {
           
            var returnD1ReadD2Carry = new ReturnDigit1ReadDigit2(carry: true, bitsPerDigit);
            var returnD2ReadD3Carry = new ReturnDigit2ReadDigit3(carry: true, bitsPerDigit);
            var returnD3ReadD1Carry = new ReturnDigit3ReadDigit1(carry: true, bitsPerDigit);

            tiles.AddRange(returnD3ReadD1Carry.Tiles);
            tiles.AddRange(returnD1ReadD2Carry.Tiles);
            tiles.AddRange(returnD2ReadD3Carry.Tiles);


            var returnD1ReadD2NoCarry = new ReturnDigit1ReadDigit2(carry: false, bitsPerDigit);
            var returnD2ReadD3NoCarry = new ReturnDigit2ReadDigit3(carry: false, bitsPerDigit);
            var returnD3ReadD1NoCarry = new ReturnDigit3ReadDigit1(carry: false, bitsPerDigit);

            tiles.AddRange(returnD3ReadD1NoCarry.Tiles);
            tiles.AddRange(returnD1ReadD2NoCarry.Tiles);
            tiles.AddRange(returnD2ReadD3NoCarry.Tiles);

            switch (digitsInMSR)
            {
                case 1:
                    var returnDigit1ReadNextRowCarry   = new ReturnDigit1ReadNextRow(carry: true, bitsPerDigit, regions);
                    var returnDigit1ReadNextRowNoCarry = new ReturnDigit1ReadNextRow(carry: false, bitsPerDigit, regions);
                    tiles.AddRange(returnDigit1ReadNextRowCarry.Tiles);
                    tiles.AddRange(returnDigit1ReadNextRowNoCarry.Tiles);
                    break;

                case 2:
                    var returnDigit2ReadNextRowCarry   = new ReturnDigit2ReadNextRow(carry: true,  bitsPerDigit, regions);
                    var returnDigit2ReadNextRowNoCarry = new ReturnDigit2ReadNextRow(carry: false, bitsPerDigit, regions);
                    tiles.AddRange(returnDigit2ReadNextRowCarry.Tiles);
                    tiles.AddRange(returnDigit2ReadNextRowNoCarry.Tiles);

                    var returnDigit1ReadDigit2Case2Carry   = new ReturnDigit1ReadDigit2Case2(carry: true, bitsPerDigit);
                    var returnDigit1ReadDigit2Case2NoCarry = new ReturnDigit1ReadDigit2Case2(carry: false, bitsPerDigit);
                    tiles.AddRange(returnDigit1ReadDigit2Case2Carry.Tiles);
                    tiles.AddRange(returnDigit1ReadDigit2Case2NoCarry.Tiles);
                    break;

                case 3:
                    var returnDigit3ReadNextRowCarry   = new ReturnDigit3ReadNextRow(carry: true,  bitsPerDigit, regions);
                    var returnDigit3ReadNextRowNoCarry = new ReturnDigit3ReadNextRow(carry: false, bitsPerDigit, regions);
                    tiles.AddRange(returnDigit3ReadNextRowCarry.Tiles);
                    tiles.AddRange(returnDigit3ReadNextRowNoCarry.Tiles);
                    break;
            }
            
        }


        private IEnumerable<Tile> CreateWriters(string bits)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new BinaryWriter(bits, true,  i, digitsInMSR).Tiles);
                results.AddRange(new BinaryWriter(bits, false, i, digitsInMSR).Tiles);
            }

            return results;
        }


        private IEnumerable<Tile> CreateWarpUnits(string bits)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new WarpUnit(bits, i, true,  digitsInMSR).Tiles);
                results.AddRange(new WarpUnit(bits, i, false, digitsInMSR).Tiles);
            }

            return results;
        }

    }
}
