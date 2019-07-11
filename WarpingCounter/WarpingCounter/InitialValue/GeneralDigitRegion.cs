namespace WarpingCounter.InitialValue
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;
    using Gadgets.Warping.FirstWarp;
    using Gadgets.Warping.PostWarp;
    using Gadgets.Warping.SecondWarp;
    using Gadgets.Warping.WarpBridge;

    using Seed;

    public class GeneralDigitRegion
    {
        private readonly int region;

        private readonly int L;

        public List<Tile> Tiles { get; } = new List<Tile>();

        public GeneralDigitRegion((string digit1, string digit2, string digit3) digits, int region, int L)
        {
            this.region = region;
            this.L = L;
            CreateDigit1(digits.digit1);
            CreateDigit2(digits.digit2);
            CreateDigit3(digits.digit3);
        }


        private void CreateDigit1(string digit)
        {
            var digit1 = new DigitWriter(digit,
                                         new Glue($"{Names.CounterWrite} {1} {Names.Seed} {region} {1}"),
                                         new Glue($"{Names.DigitTop} {1} {Names.Seed} {region} {1}"));
            Tiles.AddRange(digit1.Tiles);
            digit1.Tiles.PrependNamesWith($"Digit1Seed {region}");


            var digitTop1 = new DigitTop(L,
                                         new Glue($"{Names.DigitTop} {1} {Names.Seed} {region} {1}"),
                                         new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}"));

            digitTop1.Output.Down = new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}");
            Tiles.AddRange(digitTop1.Tiles);
            digitTop1.Tiles.PrependNamesWith($"DigitTop1Seed {region}");

            var returnPathDigit1Seed = new Tile($"ReturnPath1Seed {region} {Guid.NewGuid().ToString()}") {
                Up    = new Glue($"{Names.ReturnPath} {1} {Names.Seed} {region} {1}"),
                North = new Glue($"{Names.NextRead} {1} {Names.Seed} {region} {1}")
            };
            Tiles.Add(returnPathDigit1Seed);

            var nextReadDigit1Seed = new Tile($"NextRead1Seed {region} {Guid.NewGuid().ToString()}") { 
                South = new Glue($"{Names.NextRead} {1} {Names.Seed} {region} {1}"),
                North = new Glue($"{Names.SecondWarp} {2} {Names.Seed} {region} {2}"),
            };
            Tiles.Add(nextReadDigit1Seed);
        }


        private void CreateDigit2(string digit)
        {
            var secondWarpDigit2 = new SecondWarpDigit2(new Glue($"{Names.SecondWarp} {2} {Names.Seed} {region} {2}"),
                                                        new Glue($"{Names.SecondWarp} {2} {Names.Seed} {region} {2}"),
                                                        new Glue($"{Names.PostWarp} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(secondWarpDigit2.Tiles);
            secondWarpDigit2.Tiles.PrependNamesWith($"SecondWarp2Seed {region}");

            var postWarpDigit2 = new PostWarpDigit2(new Glue($"{Names.PostWarp} {2} {Names.Seed} {region} {2}"),
                                                    new Glue($"{Names.CounterWrite} {2} {Names.Seed} {region} {2}"));

            Tiles.AddRange(postWarpDigit2.Tiles);
            postWarpDigit2.Tiles.PrependNamesWith($"PostWarp2Seed {region}");

            var digit2 = new DigitWriter(digit,
                                         new Glue($"{Names.CounterWrite} {2} {Names.Seed} {region} {2}"),
                                         new Glue($"{Names.DigitTop} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(digit2.Tiles);
            digit2.Tiles.PrependNamesWith($"Digit2Seed {region}");

            var digitTop2 = new DigitTop(L,
                                         new Glue($"{Names.DigitTop} {2} {Names.Seed} {region} {2}"),
                                         new Glue($"{Names.ReturnPath} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(digitTop2.Tiles);
            digitTop2.Tiles.PrependNamesWith($"DigitTop2Seed {region}");

            var returnPath2 = new ReturnPathDigit2(L,
                                                   new Glue($"{Names.ReturnPath} {2} {Names.Seed} {region} {2}"),
                                                   new Glue($"{Names.NextRead} {2} {Names.Seed} {region} {2}"));
            Tiles.AddRange(returnPath2.Tiles);
            returnPath2.Tiles.PrependNamesWith($"ReturnPath2Seed {region}");

            var nextRead2 = new NextReadDigit2Seed(L,
                                                   new Glue($"{Names.NextRead} {2} {Names.Seed} {region} {2}"),
                                                   new Glue($"{Names.FirstWarp} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(nextRead2.Tiles);
            nextRead2.Tiles.PrependNamesWith($"NextRead2Seed {region}");

        }


        private void CreateDigit3(string digit)
        {
            var firstWarp3 = new FirstWarpDigit3(new Glue($"{Names.FirstWarp} {3} {Names.Seed} {region} {3}"),
                                                 new Glue($"{Names.FirstWarp} {3} {Names.Seed} {region} {3}"),
                                                 new Glue($"{Names.WarpBridge} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(firstWarp3.Tiles);
            firstWarp3.Tiles.PrependNamesWith($"FirstWarp3Seed {region}");


            var warpBridge3 = new WarpBridgeDigit3(new Glue($"{Names.WarpBridge} {3} {Names.Seed} {region} {3}"),
                                                   new Glue($"{Names.SecondWarp} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(warpBridge3.Tiles);
            warpBridge3.Tiles.PrependNamesWith($"WarpBridge3Seed {region}");



            var secondWarp3 = new SecondWarpDigit3(new Glue($"{Names.SecondWarp} {3} {Names.Seed} {region} {3}"),
                                                   new Glue($"{Names.SecondWarp} {3} {Names.Seed} {region} {3}"),
                                                   new Glue($"{Names.PostWarp} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(secondWarp3.Tiles);
            secondWarp3.Tiles.PrependNamesWith($"SecondWarp3Seed {region}");


            var postWarp3 = new PostWarpDigit3(new Glue($"{Names.PostWarp} {3} {Names.Seed} {region} {3}"),
                                               new Glue($"{Names.CounterWrite} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(postWarp3.Tiles);
            postWarp3.Tiles.PrependNamesWith($"PostWarp3Seed {region}");


            var digit3 = new DigitWriter(digit,
                                         new Glue($"{Names.CounterWrite} {3} {Names.Seed} {region} {3}"),
                                         new Glue($"{Names.DigitTop} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(digit3.Tiles);
            digit3.Tiles.PrependNamesWith($"Digit3Seed {region}");


            var digitTop3 = new DigitTop(L, 
                                         new Glue($"{Names.DigitTop} {3} {Names.Seed} {region} {3}"),
                                         new Glue($"{Names.ReturnPath} {3} {Names.Seed} {region} {3}"));
            digitTop3.Tiles.PrependNamesWith($"DigitTop3Seed {region}");
            Tiles.AddRange(digitTop3.Tiles);



            var returnPath3 = new ReturnPathDigit3(L, 
                                                   new Glue($"{Names.ReturnPath} {3} {Names.Seed} {region} {3}"),
                                                   new Glue($"{Names.NextRead} {3} {Names.Seed} {region} {3}"));
            Tiles.AddRange(returnPath3.Tiles);
            returnPath3.Tiles.PrependNamesWith($"ReturnPath3Seed {region}");



            var nextRead3 = new NextReadDigit3Seed(new Glue($"{Names.NextRead} {3} {Names.Seed} {region} {3}"), 
                                                   new Glue($"{Names.CounterWrite} {1} {Names.Seed} {region + 1} {1}"));
            Tiles.AddRange(nextRead3.Tiles);
            nextRead3.Tiles.PrependNamesWith($"NextRead3Seed {region}");
        }
    }
}
