namespace WarpingCounter.InitialValue
{

    using System;
    using System.Collections.Generic;

    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;
    using Gadgets.Warping.PostWarp;
    using Gadgets.Warping.SecondWarp;

    using Seed;

    public class Case1DigitRegion
    {

        private readonly int L;

        private readonly int region;

        public List<Tile> Tiles { get; } = new List<Tile>();

        public Case1DigitRegion(string digit1, int region, int L)
        {
            this.region = region;
            this.L = L;
            CreateDigit1(digit1);
        }


        private void CreateDigit1(string digit)
        {
            var digit1 = new DigitWriter(digit,
                                         new Glue($"{Names.CounterWrite} {1} {Names.Seed} {region} {1}"),
                                         new Glue($"{Names.DigitTop} {1} {Names.Seed} {region} {1}"));
            Tiles.AddRange(digit1.Tiles);
            digit1.Tiles.PrependNamesWith($"Digit1Seed {region}");


            var digitTop1 = new DigitTopDigit1Case1(L,
                                                    new Glue($"{Names.DigitTop} {1} {Names.Seed} {region} {1}"),
                                                    new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}"));

            Tiles.AddRange(digitTop1.Tiles);
            digitTop1.Tiles.PrependNamesWith($"DigitTop1Seed {region}");


            var returnPathDigit1Seed = new ReturnPathDigit1Case1(L,
                                                                 new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}"),
                                                                 new Glue($"{Names.NextRead} {1} {Names.Seed} {region} {1}"));
            Tiles.AddRange(returnPathDigit1Seed.Tiles);
            returnPathDigit1Seed.Tiles.PrependNamesWith($"ReturnPath1Seed {region}");


            var nextReadDigit1Seed = new NextReadDigit1Case1(L,
                                                             new Glue($"{Names.NextRead} {1} {Names.Seed} {region} {1}"),
                                                             new Glue($"{Names.CrossNextRow} {true}"));
            Tiles.AddRange(nextReadDigit1Seed.Tiles);
            nextReadDigit1Seed.Tiles.PrependNamesWith($"NextReadDigit1Seed {region}");
        }
        
    }
}
