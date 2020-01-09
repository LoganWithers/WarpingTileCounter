namespace WarpingCounter
{
    public class Op
    {
        private Op(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static Op Increment => new Op("Increment");

        public static Op Halt => new Op("Halt");

        public static Op Copy => new Op("Copy");
    }
}
