namespace WarpingCounter.InitialValue
{

    using System.Collections.Generic;

    using Common.Models;

    using Gadgets.DigitTop;
    using Gadgets.NextRead;
    using Gadgets.ReturnPath;

    using Seed;

    public class Case1DigitRegion : AbstractTileNamer
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
            var digit1 = new DigitWriter(Name(CounterWrite + Seed, 1, region, true),
                                         digit,
                                         new Glue($"{CounterWrite} {1} {Seed} {region} {1}"),
                                         new Glue($"{DigitTop} {1} {Seed} {region} {1}"));
            Tiles.AddRange(digit1.Tiles);


            var digitTop1Case1Seed = new DigitTopDigit1Case1(Name(DigitTop + Seed, 1, true, msr: true, msd: true),
                                                             L,
                                                             new Glue($"{DigitTop} {1} {Seed} {region} {1}"),
                                                             new Glue($"{ReturnPath} {1} {Seed} {region} {1}"));

            Tiles.AddRange(digitTop1Case1Seed.Tiles);


            var returnPathDigit1Seed = new ReturnPathDigit1Case1(Name(ReturnPath + Seed, 1, true, msr: true, msd: true),
                                                                 L,
                                                                 new Glue($"{ReturnPath} {1} {Seed} {region} {1}"),
                                                                 new Glue($"{NextRead} {1} {Seed} {region} {1}"));
            Tiles.AddRange(returnPathDigit1Seed.Tiles);


            var nextReadDigit1Seed = new NextReadDigit1Case1(Name(NextRead + Seed, 1, true, msr: true, msd: true),
                                                             L,
                                                             new Glue($"{NextRead} {1} {Seed} {region} {1}"),
                                                             new Glue($"{CrossNextRow} {true}"));
            Tiles.AddRange(nextReadDigit1Seed.Tiles);
        }
        
    }
}
