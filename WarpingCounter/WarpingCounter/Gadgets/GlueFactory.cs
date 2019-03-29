namespace WarpingCounter.Gadgets
{

    using Common.Models;

    /// <summary>
    /// Handles the binding of tiles between gadgets. 
    /// </summary>
    public static class GlueFactory
    {

        #region Warp units
        
        public static Glue PreFirstWarp(string bits, bool carry, int index) => new Glue($"bits={bits} carry={carry} Pre_First_Warp_D{index}");

        public static Glue FirstWarp(string bits, int digit, bool carry)    => new Glue($"First_Warp_D{digit} bits={bits} carry={carry}");

        public static Glue SecondWarp(string bits, int digit, bool carry)   => new Glue($"Second_Warp_D{digit} bits={bits} carry={carry}");

        public static Glue WarpBridge(string bits, int digit, bool carry)   => new Glue($"Warp_Bridge_D{digit} bits={bits} carry={carry}");

        public static Glue PostWarp(string bits, int digit, bool carry)     => new Glue($"Post_Warp_D{digit} bits={bits} carry={carry}");

        #endregion


        #region Digit Tops
        
        public static Glue DigitTopStart(bool carry, int index) => new Glue($"DigitTopStart carry={carry} index={index}");

        public static Glue MsdTopCase3(bool carry) => new Glue($"LastDigitTopStart carry={carry} index=3");

        public static Glue MsdTopCase2(bool carry) => new Glue($"MSDTopCase2 {carry} index=2");

        public static Glue DigitTopDigit1Case2(bool carry) => new Glue($"Digit1Case2 {carry} index=1");

        #endregion


        #region Return and read

        public static Glue ReturnD1ReadD2(bool carry)      => new Glue($"Return_D1_Read_D2 carry={carry}");

        public static Glue ReturnD1ReadD2Case2(bool carry) => new Glue($"Return_D1_Read_D2_Case_2 carry={carry}");

        public static Glue ReturnD2ReadD3(bool carry)      => new Glue($"Return_D2_Read_D3 carry={carry}");

        public static Glue ReturnD3ReadD1(bool carry)      => new Glue($"Return_D3_Read_D1 carry={carry}");

        public static Glue ReturnD3CrossReadD1(bool carry) => new Glue($"Return_D3_Cross_Read_D1 carry={carry}");

        public static Glue ReturnD2CrossReadD1(bool carry) => new Glue($"Return_D2_Cross_Read_D1 carry={carry}");

        public static Glue ReturnD1ReadD1(bool carry)      => new Glue($"Return_D1_Read_D1 carry={carry}");

        #endregion


        #region Read and write

        public static Glue WriteDigit(string bits, int digit, bool carry) => new Glue($"Write_Digit_D{digit} bits={bits} carry={carry}");

        public static Glue DigitReader(string bits, bool carry, int index) => new Glue($"DigitReader bits={bits} carry={carry} index={index}");

        #endregion
        
    }
}
