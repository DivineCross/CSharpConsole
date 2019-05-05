using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class ModelL
    {
        public int Id { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public ModelM M { get; set; }

        public List<ModelM> Ms { get ; set; }
    }

    public class ModelM
    {
        public int Id { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public ModelS S { get ; set; }

        public List<ModelS> Ss { get ; set; }
    }

    public class ModelS
    {
        public string Key { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public int Int1 { get; set; }

        public int? Int2 { get; set; }

        public decimal Dec1 { get; set; }

        public decimal? Dec2 { get; set; }

        public DateTime Date1 { get; set; }

        public DateTime? Date2 { get; set; }
    }
}
