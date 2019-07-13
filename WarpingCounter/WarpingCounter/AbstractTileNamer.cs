namespace WarpingCounter
{

    using Common.Models;

    public abstract class AbstractTileNamer
    {
        public const string CounterRead  = "CounterRead";
        public const string PreWarp      = "PreWarp";
        public const string FirstWarp    = "FirstWarp";
        public const string WarpBridge   = "WarpBridge";
        public const string SecondWarp   = "SecondWarp";
        public const string PostWarp     = "PostWarp";
        public const string CounterWrite = "CounterWrite";
        public const string DigitTop     = "DigitTop";
        public const string ReturnPath   = "ReturnPath";
        public const string NextRead     = "NextRead";
        public const string Seed         = "Seed";
        public const string CrossNextRow = "CrossNextRow";
        public const string RoofUnit     = "RoofUnit";

        public string Name(string gadget, int  i, string U,  bool op,          bool msr = false, bool msd = false) => $"{gadget} {i} '{U}' {op} {msr} {msd}";
        public string Name(string gadget, int  i, int region, bool   op, bool msr = false, bool msd = false) => $"{gadget} {i} {region} {op} {msr} {msd}";
        public string Name(string gadget, int  i, bool   op, bool msr = false, bool msd = false) => $"{gadget} {i} {op} {msr} {msd}";
        public string Name(string gadget, bool op) => $"{gadget} {op}";

        public Glue Bind(string uniqueIdentifier, int i, bool op = false, bool msr = false, bool msd = false) => new Glue($"{uniqueIdentifier} {i} {op} {msr} {msd}");

        public Glue Bind(string uniqueIdentifier, int i, string bits = "", bool op = false, bool msr = false, bool msd = false) => new Glue($"{uniqueIdentifier} {i} '{bits}' {op} {msr} {msd}");

        public Glue Bind(string uniqueIdentifier, bool op) => new Glue($"{uniqueIdentifier} {op}");
    }
}
