namespace WarpingCounter.InitialValue
{
    using System.Collections.Generic;

    using Common.Models;

    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;
    using Gadgets.Warping.FirstWarp;
    using Gadgets.Warping.PostWarp;
    using Gadgets.Warping.SecondWarp;
    using Gadgets.Warping.WarpBridge;

    public class GeneralDigitRegion : AbstractTileNamer
    {
        private readonly int region;

        private readonly int L;

        public List<Tile> Tiles { get; } = new List<Tile>();

        public GeneralDigitRegion((string digit1, string digit2, string digit3) digits, int region, int L)
        {
            this.region = region;
            this.L      = L;
            CreateDigit1(digits.digit1);
            CreateDigit2(digits.digit2);
            CreateDigit3(digits.digit3);
        }

        private void CreateDigit1(string digit)
        {
            var digit1 = new DigitWriter(Name(CounterWrite + Seed, 1, region, Op.Increment),
                                         digit,
                                         new Glue($"{CounterWrite} {1} {Seed} {region} {1}"),
                                         new Glue($"{DigitTop} {1} {Seed} {region} {1}"));

            Tiles.AddRange(digit1.Tiles);

            var digitTop1 = new DigitTop(Name(DigitTop + Seed, 1, region, Op.Increment),
                                         L,
                                         new Glue($"{DigitTop} {1} {Seed} {region} {1}"),
                                         new Glue($"{ReturnPath} {1} {Seed} {region} {1}"));

            digitTop1.Output.Down = new Glue($"{ReturnPath} {1} {Seed} {region} {1}");
            Tiles.AddRange(digitTop1.Tiles);

            var returnPathDigit1Seed = new ReturnPathDigit1Seed(Name(ReturnPath + Seed, 1, region, Op.Increment),
                                                                new Glue($"{ReturnPath} {1} {Seed} {region} {1}"),
                                                                new Glue($"{NextRead} {1} {Seed} {region} {1}"));

            Tiles.AddRange(returnPathDigit1Seed.Tiles);

            var nextReadDigit1Seed = new NextReadDigit1Seed(Name(NextRead + Seed, 1, region, Op.Increment),
                                                            new Glue($"{NextRead} {1} {Seed} {region} {1}"),
                                                            new Glue($"{SecondWarp} {2} {Seed} {region} {2}"));

            Tiles.AddRange(nextReadDigit1Seed.Tiles);
        }

        private void CreateDigit2(string digit)
        {
            var secondWarpDigit2 = new SecondWarpDigit2(Name(SecondWarp + Seed, 2, region, Op.Increment),
                                                        new Glue($"{SecondWarp} {2} {Seed} {region} {2}"),
                                                        new Glue($"{SecondWarp} {2} {Seed} {region} {2}"),
                                                        new Glue($"{PostWarp} {2} {Seed} {region} {2}"));

            Tiles.AddRange(secondWarpDigit2.Tiles);

            var postWarpDigit2 = new PostWarpDigit2(Name(PostWarp + Seed, 2, region, Op.Increment),
                                                    new Glue($"{PostWarp} {2} {Seed} {region} {2}"),
                                                    new Glue($"{CounterWrite} {2} {Seed} {region} {2}"));

            Tiles.AddRange(postWarpDigit2.Tiles);

            var digit2 = new DigitWriter(Name(CounterWrite + Seed, 2, region, Op.Increment),
                                         digit,
                                         new Glue($"{CounterWrite} {2} {Seed} {region} {2}"),
                                         new Glue($"{DigitTop} {2} {Seed} {region} {2}"));

            Tiles.AddRange(digit2.Tiles);

            var digitTop2 = new DigitTop(Name(DigitTop + Seed, 2, region, Op.Increment),
                                         L,
                                         new Glue($"{DigitTop} {2} {Seed} {region} {2}"),
                                         new Glue($"{ReturnPath} {2} {Seed} {region} {2}"));

            Tiles.AddRange(digitTop2.Tiles);

            var returnPath2 = new ReturnPathDigit2(Name(ReturnPath + Seed, 2, region, Op.Increment),
                                                   L,
                                                   new Glue($"{ReturnPath} {2} {Seed} {region} {2}"),
                                                   new Glue($"{NextRead} {2} {Seed} {region} {2}"));

            Tiles.AddRange(returnPath2.Tiles);

            var nextRead2 = new NextReadDigit2Seed(Name(NextRead + Seed, 2, region, Op.Increment),
                                                   L,
                                                   new Glue($"{NextRead} {2} {Seed} {region} {2}"),
                                                   new Glue($"{FirstWarp} {3} {Seed} {region} {3}"));

            Tiles.AddRange(nextRead2.Tiles);
        }

        private void CreateDigit3(string digit)
        {
            var firstWarp3 = new FirstWarpDigit3(Name(FirstWarp + Seed, 3, region, Op.Increment),
                                                 new Glue($"{FirstWarp} {3} {Seed} {region} {3}"),
                                                 new Glue($"{FirstWarp} {3} {Seed} {region} {3}"),
                                                 new Glue($"{WarpBridge} {3} {Seed} {region} {3}"));

            Tiles.AddRange(firstWarp3.Tiles);

            var warpBridge3 = new WarpBridgeDigit3(Name(WarpBridge + Seed, 3, region, Op.Increment),
                                                   new Glue($"{WarpBridge} {3} {Seed} {region} {3}"),
                                                   new Glue($"{SecondWarp} {3} {Seed} {region} {3}"));

            Tiles.AddRange(warpBridge3.Tiles);

            var secondWarp3 = new SecondWarpDigit3(Name(SecondWarp + Seed, 3, region, Op.Increment),
                                                   new Glue($"{SecondWarp} {3} {Seed} {region} {3}"),
                                                   new Glue($"{SecondWarp} {3} {Seed} {region} {3}"),
                                                   new Glue($"{PostWarp} {3} {Seed} {region} {3}"));

            Tiles.AddRange(secondWarp3.Tiles);

            var postWarp3 = new PostWarpDigit3(Name(PostWarp + Seed, 3, region, Op.Increment),
                                               new Glue($"{PostWarp} {3} {Seed} {region} {3}"),
                                               new Glue($"{CounterWrite} {3} {Seed} {region} {3}"));

            Tiles.AddRange(postWarp3.Tiles);

            var digit3 = new DigitWriter(Name(CounterWrite + Seed, 3, region, Op.Increment),
                                         digit,
                                         new Glue($"{CounterWrite} {3} {Seed} {region} {3}"),
                                         new Glue($"{DigitTop} {3} {Seed} {region} {3}"));

            Tiles.AddRange(digit3.Tiles);

            var digitTop3 = new DigitTop(Name(DigitTop + Seed, 3, region, Op.Increment),
                                         L,
                                         new Glue($"{DigitTop} {3} {Seed} {region} {3}"),
                                         new Glue($"{ReturnPath} {3} {Seed} {region} {3}"));

            Tiles.AddRange(digitTop3.Tiles);

            var returnPath3 = new ReturnPathDigit3(Name(ReturnPath + Seed, 3, region, Op.Increment),
                                                   L,
                                                   new Glue($"{ReturnPath} {3} {Seed} {region} {3}"),
                                                   new Glue($"{NextRead} {3} {Seed} {region} {3}"));

            Tiles.AddRange(returnPath3.Tiles);

            var nextRead3 = new NextReadDigit3Seed(Name(NextRead + Seed, 3, region, Op.Increment),
                                                   new Glue($"{NextRead} {3} {Seed} {region} {3}"),
                                                   new Glue($"{CounterWrite} {1} {Seed} {region + 1} {1}"));

            Tiles.AddRange(nextRead3.Tiles);
        }
    }
}
