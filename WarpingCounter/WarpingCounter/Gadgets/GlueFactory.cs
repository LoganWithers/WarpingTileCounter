namespace WarpingCounter.Gadgets
{

    using Common.Models;

    /// <summary>
    ///   Handles the external binding of tiles between gadgets.
    /// </summary>
    public static class GlueFactory
    {

        #region Warp units


        public static Glue PreFirstWarp(string bits, bool carry, int index) => new Glue($"bits={bits} carry={carry} Pre_First_Warp_D{index}");


        public static Glue FirstWarp(string bits, int digit, bool carry) => new Glue($"First_Warp_D{digit} bits={bits} carry={carry}");


        public static Glue SecondWarp(string bits, int digit, bool carry) => new Glue($"Second_Warp_D{digit} bits={bits} carry={carry}");


        public static Glue WarpBridge(string bits, int digit, bool carry) => new Glue($"Warp_Bridge_D{digit} bits={bits} carry={carry}");


        public static Glue PostWarp(string bits, int digit, bool carry) => new Glue($"Post_Warp_D{digit} bits={bits} carry={carry}");


        #endregion


        #region Digit Tops


        public static Glue DigitTopDefault(bool carry, int index) => new Glue($"DigitTopStart {carry} {index}");


        public static Glue DigitTopDigit3Case3(bool carry) => new Glue($"DigitTopDigit3 {carry} 3");


        public static Glue DigitTopDigit2Case2(bool carry) => new Glue($"DigitTopDigit2Case2 {carry} 2");


        public static Glue DigitTopDigit1Case2(bool carry) => new Glue($"DigitTopDigit1Case2 {carry} 1");


        #endregion


        #region Return and read


        public static Glue ReturnDigit1ReadDigit2(bool carry) => new Glue($"ReturnDigit1ReadDigit2 {carry}");


        public static Glue ReturnDigit1ReadDigit2Case2(bool carry) => new Glue($"ReturnDigit1ReadDigit2Case_2 {carry}");


        public static Glue ReturnDigit2ReadDigit3(bool carry) => new Glue($"ReturnDigit2ReadDigit3 {carry}");


        public static Glue ReturnDigit3ReadDigit1(bool carry) => new Glue($"ReturnDigit3ReadDigit1 {carry}");


        public static Glue ReturnDigit3ReadNextRow(bool carry) => new Glue($"ReturnDigit3ReadNextRow {carry}");


        public static Glue ReturnDigit2ReadNextRow(bool carry) => new Glue($"ReturnDigit2ReadNextRow {carry}");


        public static Glue ReturnDigit1ReadNextRow(bool carry) => new Glue($"ReturnDigit1ReadNextRow {carry}");


        #endregion


        #region Read and write


        public static Glue DigitWriter(string bits, bool carry, int index) => new Glue($"DigitWriter bits={bits} {carry} {index}");


        public static Glue DigitReader(string bits, bool carry, int index) => new Glue($"DigitReader bits={bits} {carry} {index}");


        #endregion

    }

}
