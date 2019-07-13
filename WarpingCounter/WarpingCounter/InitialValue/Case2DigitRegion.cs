namespace WarpingCounter.InitialValue
{

    using System.Collections.Generic;

    using Common.Models;

    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;
    using Gadgets.Warping.PostWarp;
    using Gadgets.Warping.SecondWarp;

    public class Case2DigitRegion : AbstractTileNamer
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
            var digit1 = new DigitWriter(Name(CounterWrite + Seed, 1, region, op: true, msr: true),
                                         digit,
                                         new Glue($"{CounterWrite} {1} {Seed} {region} {1}"),
                                         new Glue($"{DigitTop} {1} {Seed} {region} {1}"));
            Tiles.AddRange(digit1.Tiles);

            var digitTop1 = new DigitTopDigit1Case2(Name(DigitTop + Seed, 1, op: true, msr: true), 
                                                    L,
                                                    new Glue($"{DigitTop} {1} {Seed} {region} {1}"),
                                                    new Glue($"{ReturnPath} {1} {Seed} {region} {1}"));

            Tiles.AddRange(digitTop1.Tiles);

            var returnPathDigit1Seed = new Tile(Name(ReturnPath + Seed, 1, region, op: true, msr: true)) {
                North = new Glue($"{ReturnPath} {1} {Seed} {region} {1}"),
                East  = new Glue($"{NextRead} {1} {Seed} {region} {1}")
            };
            Tiles.Add(returnPathDigit1Seed);


            var nextReadDigit1Case2Seed = new Tile(Name(NextRead + Seed, 1, region, op: true, msr: true)) {
                West = new Glue($"{NextRead} {1} {Seed} {region} {1}"),
                North = new Glue($"{SecondWarp} {2} {Seed} {region} {2}"),
            };
            Tiles.Add(nextReadDigit1Case2Seed);
        }


        private void CreateDigit2(string digit)
        {
            var secondWarpDigit2 = new SecondWarpDigit2Case2(Name(SecondWarp + Seed, 2, region, op: true, msr: true, msd: true), 
                                                             new Glue($"{SecondWarp} {2} {Seed} {region} {2}"),
                                                             new Glue($"{SecondWarp} {2} {Seed} {region} {2}"),
                                                             new Glue($"{PostWarp} {2} {Seed} {region} {2}"));
            Tiles.AddRange(secondWarpDigit2.Tiles);

            var postWarpDigit2 = new PostWarpDigit2Case2(Name(PostWarp + Seed, 2, region, op: true, msr: true, msd: true), 
                                                         new Glue($"{PostWarp} {2} {Seed} {region} {2}"),
                                                         new Glue($"{CounterWrite} {2} {Seed} {region} {2}"));

            Tiles.AddRange(postWarpDigit2.Tiles);

            var digit2 = new DigitWriter(Name(CounterWrite + Seed, 2, region, op: true, msr: true, msd: true),
                                         digit,
                                         new Glue($"{CounterWrite} {2} {Seed} {region} {2}"),
                                         new Glue($"{DigitTop} {2} {Seed} {region} {2}"));
            Tiles.AddRange(digit2.Tiles);

            var digitTop2 = new DigitTopDigit2Case2(Name(DigitTop + Seed, 2, op: true, msr: true, msd: true), 
                                                    L,
                                                    new Glue($"{DigitTop} {2} {Seed} {region} {2}"),
                                                    new Glue($"{ReturnPath} {2} {Seed} {region} {2}"));
            Tiles.AddRange(digitTop2.Tiles);

            var returnPath2 = new ReturnPathDigit2Case2(Name(ReturnPath + Seed, 2, region, op: true, msr: true, msd: true), 
                                                        L,
                                                        new Glue($"{ReturnPath} {2} {Seed} {region} {2}"),
                                                        new Glue($"{NextRead} {2} {Seed} {region} {2}"));
            Tiles.AddRange(returnPath2.Tiles);

            var nextRead2 = new NextReadDigit2Case2(Name(NextRead + Seed, 2, region, op: true, msr: true, msd: true), 
                                                    L,
                                                    new Glue($"{NextRead} {2} {Seed} {region} {2}"),
                                                    new Glue($"{CrossNextRow} {true}"));
            Tiles.AddRange(nextRead2.Tiles);
        }


    }
}

