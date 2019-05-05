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
            var lm = new ModelL {
                Id = 42,
                Str1 = "L.Str1",
                Str2 = "L.Str2",
                M = null,
                Ms = new List<ModelM> {
                    new ModelM {
                        Id = 1,
                        Str1 = "L.Ms[0,1].Str1",
                        Str2 = "L.Ms[0,1].Str2",
                    },
                    new ModelM {
                        Id = 2,
                        Str1 = "L.Ms[1,2].Str1",
                        Str2 = "L.Ms[1,2].Str2",
                    },
                    new ModelM {
                        Id = 3,
                        Str1 = "L.Ms[2,3].Str1",
                        Str2 = "L.Ms[2,3].Str2",
                    },
                },
            };
            var rm = new ModelL {
                Id = 42,
                Str1 = "R.Str1",
                Str2 = "R.Str2",
                M = new ModelM {
                    Str1 = "R.M.Str1",
                    Str2 = "R.M.Str2",
                    S = new ModelS {
                        Int1 = 99,
                    },
                },
                Ms = new List<ModelM> {
                    new ModelM {
                        Id = 2,
                        Str1 = "R.Ms[0,2].Str1",
                        Str2 = "R.Ms[0,2].Str2",
                    },
                    new ModelM {
                        Id = 3,
                        Str1 = "R.Ms[1,3].Str1",
                        Str2 = "R.Ms[1,3].Str2",
                    },
                    new ModelM {
                        Id = 4,
                        Str1 = "R.Ms[2,4].Str1",
                        Str2 = "R.Ms[2,4].Str2",
                    },
                },
            };

            var mergerS = Merger<ModelS>.Create()
                .Prop(_ => _.Int1)
                .Prop(_ => _.Int2);

            var mergerM = Merger<ModelM>.Create()
                .Prop(_ => _.Id)
                .Prop(_ => _.Str1)
                .Prop(_ => _.Str2)
                .Prop(_ => _.S, mergerS);

            var mergerL = Merger<ModelL>.Create()
                .Prop(_ => _.Str1)
                .Prop(_ => _.Str2)
                .Prop(_ => _.M, mergerM)
                .PropFull(_ => _.Ms, _ => _.Id, mergerM);

            Benchmark.Run(test, 10000);

            Console.WriteLine(lm.Str1);
            Console.WriteLine(lm.Str2);
            Console.WriteLine(lm.M?.Str1);
            Console.WriteLine(lm.M?.Str2);
            Console.WriteLine(lm.M?.S?.Int1);
            if (lm.Ms.Count > 0) showM(lm.Ms[0]);
            if (lm.Ms.Count > 1) showM(lm.Ms[1]);
            if (lm.Ms.Count > 2) showM(lm.Ms[2]);
            if (lm.Ms.Count > 3) showM(lm.Ms[3]);
            if (lm.Ms.Count > 4) showM(lm.Ms[4]);
            if (lm.Ms.Count > 5) showM(lm.Ms[5]);

            void test() => mergerL.Merge(ref lm, rm);

            void showM(ModelM m)
            {
                Console.WriteLine("..ModelM..");
                Console.WriteLine(m.Id);
                Console.WriteLine(m.Str1);
                Console.WriteLine(m.Str2);
            };
        }
    }
}
