namespace WarpingCounter
{
    using Common.Models;

    public abstract class AbstractTileNamer
    {
        protected const string CounterRead = "CounterRead";

        protected const string PreWarp = "PreWarp";

        protected const string FirstWarp = "FirstWarp";

        protected const string WarpBridge = "WarpBridge";

        protected const string SecondWarp = "SecondWarp";

        protected const string PostWarp = "PostWarp";

        protected const string CounterWrite = "CounterWrite";

        protected const string DigitTop = "DigitTop";

        protected const string ReturnPath = "ReturnPath";

        protected const string NextRead = "NextRead";

        protected const string Seed = "Seed";

        protected const string CrossNextRow = "CrossNextRow";

        protected const string RoofUnit = "RoofUnit";

        public string Name(string gadget, int i, string U, Op op, bool msr = false, bool msd = false) => $"{gadget} {i} '{U}' {op.Value} {msr} {msd}";

        protected string Name(string gadget, int i, int region, Op op, bool msr = false, bool msd = false) => $"{gadget} {i} {region} {op.Value} {msr} {msd}";

        protected string Name(string gadget, int i, Op op, bool msr = false, bool msd = false) => $"{gadget} {i} {op.Value} {msr} {msd}";

        protected string Name(string gadget, Op op) => $"{gadget} {op.Value}";

        protected string Name(string gadget, int i, Op op, bool msr, bool msd, string bits) => $"{gadget} {i} '{bits}' {op.Value} {msr} {msd}";

        protected Glue Bind(string uniqueIdentifier, int i, Op op, bool msr = false, bool msd = false) => new Glue($"{uniqueIdentifier} {i} {op.Value} {msr} {msd}");

        protected Glue Bind(string uniqueIdentifier, int i, Op op, bool msr, bool msd, string bits) => new Glue($"{uniqueIdentifier} {i} '{bits}' {op.Value} {msr} {msd}");

        protected Glue Bind(string uniqueIdentifier, Op op) => new Glue($"{uniqueIdentifier} {op.Value}");
    }
}
