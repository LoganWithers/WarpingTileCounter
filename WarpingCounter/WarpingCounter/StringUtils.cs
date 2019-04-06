namespace WarpingCounter
{

    using System;

    public static class StringUtils
    {

        public static string Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

    }

}
