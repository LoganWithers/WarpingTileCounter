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

        public string Name(string gadget, int  i, string U,  Op op,  bool msr = false, bool msd = false) => $"{gadget} {i} '{U}' {op.Value} {msr} {msd}";
        public string Name(string gadget, int  i, int region, Op op, bool msr = false, bool msd = false) => $"{gadget} {i} {region} {op.Value} {msr} {msd}";
        public string Name(string gadget, int  i, Op op, bool msr = false, bool msd = false) => $"{gadget} {i} {op.Value} {msr} {msd}";
        public string Name(string gadget, Op op) => $"{gadget} {op.Value}";
        public string Name(string gadget, int i, Op op, bool msr, bool msd, string bits) => $"{gadget} {i} '{bits}' {op.Value} {msr} {msd}";

        public Glue Bind(string uniqueIdentifier, int i, Op op, bool msr = false, bool msd = false) => new Glue($"{uniqueIdentifier} {i} {op.Value} {msr} {msd}");

        public Glue Bind(string uniqueIdentifier, int i, Op op, bool msr, bool msd, string bits) => new Glue($"{uniqueIdentifier} {i} '{bits}' {op.Value} {msr} {msd}");

        public Glue Bind(string uniqueIdentifier, Op op) => new Glue($"{uniqueIdentifier} {op.Value}");
    }

    public class Op
    {
        private Op(string value) { Value = value; }

        public string Value { get; set; }

        public static Op Increment => new Op("Increment");
        public static Op Halt => new Op("Halt");
        public static Op Copy => new Op("Copy");

    }
}
