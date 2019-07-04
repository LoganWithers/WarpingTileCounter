namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnAndRead.NextDigit;
    using Gadgets.ReturnAndRead.NextRow;
    using Gadgets.Warping;

    using Seed;

    /// <summary>
    /// Provided the details of a counter, this creates all the gadgets needed to
    /// assemble a counter with a specific base and starting value. Namely, the
    /// following "gadgets" will be created.
    ///
    /// 
    /// </summary>
    public class TileGenerator
    {

        private const int Digits = 3;

        private readonly ConstructionValues construction;
        private readonly List<Tile> tiles = new List<Tile>();

        private readonly int digitsInMSR;
        private readonly int L;
        private readonly int bitsRequiredForBaseM;
        private readonly int regions;

        private readonly int baseM;

        public TileGenerator(int baseM, string initialValueB10)
        {
            this.baseM           = baseM;
            construction         = new ConstructionValues(initialValueB10, baseM);
            digitsInMSR          = construction.DigitsInMSR;
            L                    = construction.L;
            bitsRequiredForBaseM = construction.BitsRequiredForBaseM;
            regions              = construction.DigitRegions;

            Console.WriteLine($"Bits Per Counter Digit: {bitsRequiredForBaseM}");
            Console.WriteLine($"Actual Bits Per Digit:  {L}");
        }

        public bool IsStartingValueTooSmall() => construction.DigitRegions < 2;


        public List<Tile> Generate()
        {
            //    AddSeed();
            //    AddCounter();
            //    AddDigitTops();
            //    AddReturnAndRead();
            tiles.Add(new Tile("seed"){ South = GlueFactory.Create(Names.NextRead, 3, true)});
            NextRead();
            return tiles;
        }
        
        private void AddSeed()
        {
            tiles.AddRange(new SeedCreator(construction).Tiles);
        }


        private void AddCounter()
        {
            var readerFactory = new ReaderFactory(L, bitsRequiredForBaseM, baseM);
            tiles.AddRange(readerFactory.Readers.SelectMany(reader => reader.Tiles));

            List<string> fullSizeDigits = readerFactory.DigitsWithLengthL;
            
            
            Console.WriteLine($"Full Digits: {fullSizeDigits.Count}");

            // Foreach digit in {0, 1}^l
            foreach (var lengthLDigit in fullSizeDigits)
            {
                tiles.AddRange(CreateWarpUnits(lengthLDigit));
                tiles.AddRange(CreateWriters(lengthLDigit));
            }
        }

        /// <summary>
        /// Adds the top of the digits, (constant 3 for each digit index in a standard region)
        ///
        /// If it's case 2 or case 3, special digit tops are required due to
        /// paths being slightly different. 
        /// </summary>
        private void AddDigitTops()
        {
            foreach (var op in new []{true, false})
            {
                tiles.AddRange(new DigitTop(L,
                                            GlueFactory.Create(Names.DigitTop,   1, op),
                                            GlueFactory.Create(Names.ReturnPath, 1, op)).Tiles);

                tiles.AddRange(new DigitTop(L, 
                                            GlueFactory.Create(Names.DigitTop,   2, op), 
                                            GlueFactory.Create(Names.ReturnPath, 2, op)).Tiles);

                tiles.AddRange(new DigitTop(L,
                                            GlueFactory.Create(Names.DigitTop,   3, op),
                                            GlueFactory.Create(Names.ReturnPath, 3, op)).Tiles);


                switch (digitsInMSR)
                {
                    case 1:
                        tiles.AddRange(new DigitTopDigit1Case1(L,
                                                               GlueFactory.Create(Names.DigitTop,   1, op, msr: true, msd: true),
                                                               GlueFactory.Create(Names.ReturnPath, 1, op, msr: true, msd: true)).Tiles);
                        break;
        
                    case 2:
                        tiles.AddRange(new DigitTopDigit1Case2(L, 
                                                               GlueFactory.Create(Names.DigitTop,   1, op,  msr: true), 
                                                               GlueFactory.Create(Names.ReturnPath, 1, op,  msr: true)).Tiles);

                        tiles.AddRange(new DigitTopDigit2Case2(L, 
                                                               GlueFactory.Create(Names.DigitTop,   2, op, msr: true, msd: true),
                                                               GlueFactory.Create(Names.ReturnPath, 2, op, msr: true, msd: true)).Tiles);
                        break;
                    case 3:
                        tiles.AddRange(new DigitTop(L,
                                                    GlueFactory.Create(Names.DigitTop,   3, op, msr: true, msd: true),
                                                    GlueFactory.Create(Names.ReturnPath, 3, op, msr: true, msd: true)).Tiles);
                        break;
                }

            }
        
        }


        private void NextRead()
        {
            foreach (var op in new[] {true, false})
            {
                tiles.AddRange(new NextReadDigit1(L,
                                                  GlueFactory.Create(Names.NextRead, 1, op), 
                                                  GlueFactory.Create(Names.CounterRead, 2, op)).Tiles);

                tiles.AddRange(new NextReadDigit1Case2(L,
                                                       GlueFactory.Create(Names.NextRead, 1, op, msr: true),
                                                       GlueFactory.Create(Names.CounterRead, 2, op)).Tiles);

                tiles.AddRange(new NextReadDigit1Case1(L,
                                                       GlueFactory.Create(Names.NextRead, 1, op, msr: true, msd: true),
                                                       GlueFactory.Create(Names.CrossNextRow, 2, op)).Tiles);

                tiles.AddRange(new NextReadDigit2(L,
                                                  GlueFactory.Create(Names.NextRead, 2, op),
                                                  GlueFactory.Create(Names.CounterRead, 3, op)).Tiles);

                tiles.AddRange(new NextReadDigit2Case2(L,
                                                       GlueFactory.Create(Names.NextRead, 2, op, msr: true, msd: true),
                                                       GlueFactory.Create(Names.CrossNextRow, 3, op)).Tiles);

                tiles.AddRange(new NextReadDigit3(L,
                                                  GlueFactory.Create(Names.NextRead, 3, op),
                                                  GlueFactory.Create(Names.CounterRead, 1, op)).Tiles);

                tiles.AddRange(new NextReadDigit3Case3(L,
                                                       GlueFactory.Create(Names.NextRead, 3, op, msr: true, msd: true),
                                                       GlueFactory.Create(Names.CounterRead, 1, op)).Tiles);
            }
        }


        private void AddReturnAndRead()
        {
           
            var returnD1ReadD2Carry = new ReturnDigit1ReadDigit2(carry: true, bits: L);
            var returnD2ReadD3Carry = new ReturnDigit2ReadDigit3(carry: true, bits: L);
            var returnD3ReadD1Carry = new ReturnDigit3ReadDigit1(carry: true, bits: L);

            tiles.AddRange(returnD3ReadD1Carry.Tiles);
            tiles.AddRange(returnD1ReadD2Carry.Tiles);
            tiles.AddRange(returnD2ReadD3Carry.Tiles);


            var returnD1ReadD2NoCarry = new ReturnDigit1ReadDigit2(carry: false, bits: L);
            var returnD2ReadD3NoCarry = new ReturnDigit2ReadDigit3(carry: false, bits: L);
            var returnD3ReadD1NoCarry = new ReturnDigit3ReadDigit1(carry: false, bits: L);

            tiles.AddRange(returnD3ReadD1NoCarry.Tiles);
            tiles.AddRange(returnD1ReadD2NoCarry.Tiles);
            tiles.AddRange(returnD2ReadD3NoCarry.Tiles);

            switch (digitsInMSR)
            {
                case 1:
                    var returnDigit1ReadNextRowCarry   = new ReturnDigit1ReadNextRow(carry: true,  bits: L, numberOfRegions: regions);
                    var returnDigit1ReadNextRowNoCarry = new ReturnDigit1ReadNextRow(carry: false, bits: L, numberOfRegions: regions);
                    tiles.AddRange(returnDigit1ReadNextRowCarry.Tiles);                            
                    tiles.AddRange(returnDigit1ReadNextRowNoCarry.Tiles);
                    break;

                case 2:
                    var returnDigit2ReadNextRowCarry   = new ReturnDigit2ReadNextRow(carry: true,  bits: L, numberOfRegions: regions);
                    var returnDigit2ReadNextRowNoCarry = new ReturnDigit2ReadNextRow(carry: false, bits: L, numberOfRegions: regions);
                    tiles.AddRange(returnDigit2ReadNextRowCarry.Tiles);
                    tiles.AddRange(returnDigit2ReadNextRowNoCarry.Tiles);

                    var returnDigit1ReadDigit2Case2Carry   = new ReturnDigit1ReadDigit2Case2(carry: true,  bits: L);
                    var returnDigit1ReadDigit2Case2NoCarry = new ReturnDigit1ReadDigit2Case2(carry: false, bits: L);
                    tiles.AddRange(returnDigit1ReadDigit2Case2Carry.Tiles);
                    tiles.AddRange(returnDigit1ReadDigit2Case2NoCarry.Tiles);
                    break;

                case 3:
                    var returnDigit3ReadNextRowCarry   = new ReturnDigit3ReadNextRow(carry: true,  bits: L, regions);
                    var returnDigit3ReadNextRowNoCarry = new ReturnDigit3ReadNextRow(carry: false, bits: L, regions);
                    tiles.AddRange(returnDigit3ReadNextRowCarry.Tiles);
                    tiles.AddRange(returnDigit3ReadNextRowNoCarry.Tiles);
                    break;
            }
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Creates 6 total warp units.
        /// 
        /// For each digit index (i..3), 
        ///     create 1 warp unit with an increment signal
        ///     create 1 warp unit with a copy signal
        /// 
        /// </summary>
        /// <param name="bits">A base 2 encoded digit, with 2 bits padded to the end for indicating MSR/MSD</param>
        /// <returns></returns>
        private IEnumerable<Tile> CreateWarpUnits(string bits)
        {
            var results = new List<Tile>();

            for (var i = 1; i <= Digits; i++)
            {
                results.AddRange(new WarpUnit(digitValueToWrite: bits, digitIndex: i, carry: true,  digitsInMSR).Tiles);
                results.AddRange(new WarpUnit(digitValueToWrite: bits, digitIndex: i, carry: false, digitsInMSR).Tiles);
            }

            return results;
        }

    }
}
