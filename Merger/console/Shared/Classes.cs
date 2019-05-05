using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class Approval
    {
        public int Id { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? Period { get; set; }

        public int? ApplyType { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public string Str3 { get; set; }

        public int Int1 { get; set; }

        public int Int2 { get; set; }

        public int Int3 { get; set; }

        public Quota Quota { get; set; }

        public List<Quota> Quotas { get ; set; }
    }

    public class Quota
    {
        public int Id { get; set; }

        public decimal? Amount { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public decimal Dec1 { get; set; }

        public decimal Dec2 { get; set; }

        public Rate Rate { get ; set; }

        public List<Rate> Rates { get ; set; }
    }

    public class Rate
    {
        public decimal? Annual { get; set; }
    }
}
