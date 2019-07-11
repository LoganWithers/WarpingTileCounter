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

    public class Case2DigitRegion
    {

        private readonly int L;

        private readonly int region;

        public List<Tile> Tiles { get; } = new List<Tile>();

        public Case2DigitRegion((string digit1, string digit2) digits, int region, int L)
        {
            this.region = region;
            this.L      = L;
            CreateDigit1(digits.digit1);
            CreateDigit2(digits.digit2);
        }


        private void CreateDigit1(string digit)
        {
            var digit1 = new DigitWriter(digit,
                                         new Glue($"{Names.CounterWrite} {1} {Names.Seed} {region} {1}"),
                                         new Glue($"{Names.DigitTop} {1} {Names.Seed} {region} {1}"));
            Tiles.AddRange(digit1.Tiles);
            digit1.Tiles.PrependNamesWith($"Digit1Seed {region}");


            var digitTop1 = new DigitTopDigit1Case2(L,
                                                    new Glue($"{Names.DigitTop} {1} {Names.Seed} {region} {1}"),
                                                    new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}"));

            Tiles.AddRange(digitTop1.Tiles);
            digitTop1.Tiles.PrependNamesWith($"DigitTop1Seed {region}");

            var returnPathDigit1Seed = new Tile($"ReturnPath1Seed {region} {Guid.NewGuid().ToString()}") {
                North = new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}"),
                East  = new Glue($"{Names.NextRead} {1} {Names.Seed} {region} {1}")
            };
            Tiles.Add(returnPathDigit1Seed);


            var nextReadDigit1Seed = new Tile($"NextRead1Seed {region} {Guid.NewGuid().ToString()}") {
                West = new Glue($"{Names.NextRead} {1} {Names.Seed} {region} {1}"),
                North = new Glue($"{Names.SecondWarp} {2} {Names.Seed} {region} {2}"),
            };
            Tiles.Add(nextReadDigit1Seed);
        }


        private void CreateDigit2(string digit)
        {
            var secondWarpDigit2 = new SecondWarpDigit2Case2(new Glue($"{Names.SecondWarp} {2} {Names.Seed} {region} {2}"),
                                                             new Glue($"{Names.SecondWarp} {2} {Names.Seed} {region} {2}"),
                                                             new Glue($"{Names.PostWarp} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(secondWarpDigit2.Tiles);
            secondWarpDigit2.Tiles.PrependNamesWith($"SecondWarp2Seed {region}");

            var postWarpDigit2 = new PostWarpDigit2Case2(new Glue($"{Names.PostWarp} {2} {Names.Seed} {region} {2}"),
                                                         new Glue($"{Names.CounterWrite} {2} {Names.Seed} {region} {2}"));

            Tiles.AddRange(postWarpDigit2.Tiles);
            postWarpDigit2.Tiles.PrependNamesWith($"PostWarp2Seed {region}");

            var digit2 = new DigitWriter(digit,
                                         new Glue($"{Names.CounterWrite} {2} {Names.Seed} {region} {2}"),
                                         new Glue($"{Names.DigitTop} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(digit2.Tiles);
            digit2.Tiles.PrependNamesWith($"Digit2Seed {region}");

            var digitTop2 = new DigitTopDigit2Case2(L,
                                                    new Glue($"{Names.DigitTop} {2} {Names.Seed} {region} {2}"),
                                                    new Glue($"{Names.ReturnPath} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(digitTop2.Tiles);
            digitTop2.Tiles.PrependNamesWith($"DigitTop2Seed {region}");

            var returnPath2 = new ReturnPathDigit2Case2(L,
                                                        new Glue($"{Names.ReturnPath} {2} {Names.Seed} {region} {2}"),
                                                        new Glue($"{Names.NextRead} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(returnPath2.Tiles);
            returnPath2.Tiles.PrependNamesWith($"ReturnPath2Seed {region}");

            var nextRead2 = new NextReadDigit2Case2(L,
                                                    new Glue($"{Names.NextRead} {2} {Names.Seed} {region} {2}"),
                                                    new Glue($"{Names.CrossNextRow} {true}"));
            Tiles.AddRange(nextRead2.Tiles);
            returnPath2.Tiles.PrependNamesWith($"NextRead2Seed {region}");
        }


    }
}

