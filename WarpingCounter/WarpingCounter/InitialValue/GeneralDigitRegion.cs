namespace WarpingCounter.InitialValue
{

    using System;
    using System.Collections.Generic;

    using Common.Models;

    using Gadgets;
    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;
    using Gadgets.Warping.FirstWarp;
    using Gadgets.Warping.PostWarp;
    using Gadgets.Warping.SecondWarp;
    using Gadgets.Warping.WarpBridge;

    using Newtonsoft.Json.Linq;

    using Seed;

    public class GeneralDigitRegion
    {
         
        private (string digit1, string digit2, string digit3) digits;

        private readonly int regionIndex;

        private readonly int L;

        public List<Tile> Tiles { get; } = new List<Tile>();

        public GeneralDigitRegion((string digit1, string digit2, string digit3) digits, int region, int L)
        {
            this.digits = digits;
            this.regionIndex = region;
            this.L = L;
            Create();
        }


        void Create()
        {
            var i = 1;
            var digit1    = new DigitWriter(digits.digit1, 
                                            new Glue($"{Names.CounterWrite} {1} {Names.Seed} {regionIndex} {i}"),
                                            new Glue($"{Names.DigitTop} {1} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(digit1.Tiles);

            var digitTop1 = new DigitTop(L,
                                         new Glue($"{Names.DigitTop} {1} {Names.Seed} {regionIndex} {i}"),
                                         new Glue($"{Names.ReturnPath} {1} {Names.Seed} {regionIndex} {i}"));

            digitTop1.Output.Down = new Glue($"{Names.ReturnPath} {1} {Names.Seed} {regionIndex} {i}");

            Tiles.AddRange(digitTop1.Tiles);


            var returnPathDigit1Seed = new Tile(Guid.NewGuid().ToString()) {
                Up    = new Glue($"{Names.ReturnPath} {1} {Names.Seed} {regionIndex} {i}"),
                North = new Glue($"{Names.NextRead} {1} {Names.Seed} {regionIndex} {i}")
            };

            Tiles.Add(returnPathDigit1Seed);

            i++;


            var nextReadDigit1Seed = new Tile(Guid.NewGuid().ToString()) {
                South = new Glue($"{Names.NextRead} {1} {Names.Seed} {regionIndex} {i - 1}"),
                North = new Glue($"{Names.SecondWarp} {2} {Names.Seed} {regionIndex} {i}"),
            };

            Tiles.Add(nextReadDigit1Seed);


            var secondWarpDigit2 = new SecondWarpDigit2(new Glue($"{Names.SecondWarp} {2} {Names.Seed} {regionIndex} {i}"),
                                                        new Glue($"{Names.SecondWarp} {2} {Names.Seed} {regionIndex} {i}"),
                                                        new Glue($"{Names.PostWarp} {2} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(secondWarpDigit2.Tiles);


            var postWarpDigit2 = new PostWarpDigit2(new Glue($"{Names.PostWarp} {2} {Names.Seed} {regionIndex} {i}"),
                                                    new Glue($"{Names.CounterWrite} {2} {Names.Seed} {regionIndex} {i}"));

            Tiles.AddRange(postWarpDigit2.Tiles);


            var digit2 = new DigitWriter(digits.digit2,
                                         new Glue($"{Names.CounterWrite} {2} {Names.Seed} {regionIndex} {i} "),
                                         new Glue($"{Names.DigitTop} {2} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(digit2.Tiles);


            var digitTop2 = new DigitTop(L,
                                         new Glue($"{Names.DigitTop} {2} {Names.Seed} {regionIndex} {i}"),
                                         new Glue($"{Names.ReturnPath} {2} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(digitTop2.Tiles);


            var returnPath2 = new ReturnPathDigit2(L, 
                                                   new Glue($"{Names.ReturnPath} {2} {Names.Seed} {regionIndex} {i}"), 
                                                   new Glue($"{Names.ReturnPath} {2} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(returnPath2.Tiles);


            i++;


            var nextRead2 = new NextReadDigit2Seed(L,
                                                   new Glue($"{Names.ReturnPath} {2} {Names.Seed} {regionIndex} {i - 1}"),
                                                   new Glue($"{Names.FirstWarp} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(nextRead2.Tiles);

            var firstWarp3 = new FirstWarpDigit3(new Glue($"{Names.FirstWarp} {3} {Names.Seed} {regionIndex} {i}"),
                                                 new Glue($"{Names.FirstWarp} {3} {Names.Seed} {regionIndex} {i}"),
                                                 new Glue($"{Names.WarpBridge} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(firstWarp3.Tiles);

            var warpBridge3 = new WarpBridgeDigit3(new Glue($"{Names.WarpBridge} {3} {Names.Seed} {regionIndex} {i}"),
                                                   new Glue($"{Names.SecondWarp} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(warpBridge3.Tiles);
            var secondWarp3 = new SecondWarpDigit3(new Glue($"{Names.SecondWarp} {3} {Names.Seed} {regionIndex} {i}"),
                                                   new Glue($"{Names.SecondWarp} {3} {Names.Seed} {regionIndex} {i}"),
                                                   new Glue($"{Names.PostWarp} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(secondWarp3.Tiles);

            var postWarp3 = new PostWarpDigit3(new Glue($"{Names.PostWarp} {3} {Names.Seed} {regionIndex} {i}"),
                                               new Glue($"{Names.CounterWrite} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(postWarp3.Tiles);

            var digit3 = new DigitWriter(digits.digit3,
                                         new Glue($"{Names.CounterWrite} {3} {Names.Seed} {regionIndex} {i} "),
                                         new Glue($"{Names.DigitTop} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(digit3.Tiles);

            var digitTop3 = new DigitTop(L, 
                                         new Glue($"{Names.DigitTop} {3} {Names.Seed} {regionIndex} {i}"),
                                         new Glue($"{Names.ReturnPath} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(digitTop3.Tiles);

            var returnPath3 = new ReturnPathDigit3(L, 
                                                   new Glue($"{Names.ReturnPath} {3} {Names.Seed} {regionIndex} {i}"),
                                                   new Glue($"{Names.NextRead} {3} {Names.Seed} {regionIndex} {i}"));
            Tiles.AddRange(returnPath3.Tiles);

            i++;

            var nextRead3 = new NextReadDigit3Seed(new Glue($"{Names.NextRead} {3} {Names.Seed} {regionIndex} {i - 1}"), 
                                                   new Glue($"{Names.CounterWrite} {3} {Names.Seed} {regionIndex + 1} {1}"));
            Tiles.AddRange(nextRead3.Tiles);

        }
    }
}
