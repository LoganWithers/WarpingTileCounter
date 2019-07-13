﻿namespace WarpingCounter.Common.Models
{

    public class Glue
    {

        public Glue(string label)
        {
            Label = label.Replace("True", "true")
                         .Replace("False", "false");

            Bind = 1;
        }


        public Glue() { }
        
        public int Bind { get; set; }

        private string label;

        public string Label
        {
            get => label;
            set => label = value;//Regex.Replace(value, @"\s+", "");
        }
    }

}
