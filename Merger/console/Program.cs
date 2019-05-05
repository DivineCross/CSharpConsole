using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public static class Program
    {
        public static void Main()
        {
            var la = new Approval {
                Id = 87,
                TotalAmount = 111m,
                Period = 94,
                Str1 = "la.Str1",
                Int1 = 9991111,
                Quota = null,
                Quotas = new List<Quota>{
                    new Quota {
                        Id = 1,
                        Str1 = "Quota-L1.1",
                        Str2 = "Quota-L1.2",
                    },
                    new Quota {
                        Id = 2,
                        Str1 = "Quota-L2.1",
                        Str2 = "Quota-L2.2",
                    },
                    new Quota {
                        Id = 3,
                        Str1 = "Quota-L3.1",
                        Str2 = "Quota-L3.2",
                    },
                },
            };
            var ra = new Approval {
                Id = 87,
                TotalAmount = 222m,
                Period = 22,
                Str1 = "StrValue1",
                Str2 = "StrValue2",
                Str3 = "StrValue3",
                Int1 = 1,
                Int2 = 2,
                Int3 = 3,
                Quota = new Quota {
                    Str1 = "QuotaStr1",
                    Str2 = "QuotaStr2",
                    Dec1 = 948701m,
                    Dec2 = 948702m,
                    Rate = new Rate {
                        Annual = 2.22m,
                    },
                },
                Quotas = new List<Quota>{
                    new Quota {
                        Id = 2,
                        Str1 = "Quota-R2.1",
                        Str2 = "Quota-R2.2",
                        Dec1 = 9902m,
                    },
                    new Quota {
                        Id = 3,
                        Str1 = "Quota-R3.1",
                        Str2 = "Quota-R3.2",
                        Dec1 = 9903m,
                    },
                    new Quota {
                        Id = 4,
                        Str1 = "Quota-R4.1",
                        Str2 = "Quota-R4.2",
                        Dec1 = 9904m,
                    },
                },
            };

            var mergerQ = Merger<Quota>.Create()
                .Prop(_ => _.Id)
                .Prop(_ => _.Str1)
                .Prop(_ => _.Str2)
                .Prop(_ => _.Dec1)
                .Prop(_ => _.Dec2)
                .Prop(_ => _.Rate, Merger<Rate>.Create()
                    .Prop(_ => _.Annual));

            var merger = Merger<Approval>.Create()
                .Prop(_ => _.TotalAmount)
                .Prop(_ => _.Period)
                .Prop(_ => _.Str1)
                .Prop(_ => _.Str2)
                .Prop(_ => _.Str3)
                .Prop(_ => _.Int1)
                .Prop(_ => _.Int2)
                .Prop(_ => _.Int3)
                .Prop(_ => _.Quota, mergerQ)
                .PropFull(_ => _.Quotas, _ => _.Id, mergerQ);

            Action test = () => {
                merger.Merge(ref la, ra);
            };

            Benchmark.Run(test, 10000);

            Console.WriteLine(la.TotalAmount);
            Console.WriteLine(la.Period);
            Console.WriteLine(la.Str1);
            Console.WriteLine(la.Str2);
            Console.WriteLine(la.Str3);
            Console.WriteLine(la.Int1);
            Console.WriteLine(la.Int2);
            Console.WriteLine(la.Int3);

            Console.WriteLine(la.Quota?.Str1);
            Console.WriteLine(la.Quota?.Str2);
            Console.WriteLine(la.Quota?.Dec1);
            Console.WriteLine(la.Quota?.Dec2);

            Console.WriteLine(la.Quota?.Rate?.Annual);

            Action<Quota> showQuota = q => {
                Console.WriteLine("..Quota..");
                Console.WriteLine(q.Id);
                Console.WriteLine(q.Str1);
                Console.WriteLine(q.Str2);
                Console.WriteLine(q.Dec1);
            };

            if (la.Quotas.Count > 0) showQuota(la.Quotas[0]);
            if (la.Quotas.Count > 1) showQuota(la.Quotas[1]);
            if (la.Quotas.Count > 2) showQuota(la.Quotas[2]);
            if (la.Quotas.Count > 3) showQuota(la.Quotas[3]);
            if (la.Quotas.Count > 4) showQuota(la.Quotas[4]);
            if (la.Quotas.Count > 5) showQuota(la.Quotas[5]);
        }
    }
}
