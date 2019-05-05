using System;
using System.Collections.Generic;

using Xunit;

namespace ConsoleApplication.Test
{
    public class SimpleModelTest
    {
        private ModelS lsData;

        private ModelS rsData;

        private ModelS ls;

        private ModelS rs;

        private Merger<ModelS> NewMergerS => Merger<ModelS>.Create();

        [Fact]
        public void SingleProp()
        {
            Merge(NewMergerS.Prop(_ => _.Str1));
            Assert.Equal(rsData.Str1, ls.Str1);

            Merge(NewMergerS.Prop(_ => _.Str2));
            Assert.Equal(rsData.Str2, ls.Str2);

            Merge(NewMergerS.Prop(_ => _.Int1));
            Assert.Equal(rsData.Int1, ls.Int1);

            Merge(NewMergerS.Prop(_ => _.Int2));
            Assert.Equal(rsData.Int2, ls.Int2);

            Merge(NewMergerS.Prop(_ => _.Dec1));
            Assert.Equal(rsData.Dec1, ls.Dec1);

            Merge(NewMergerS.Prop(_ => _.Dec2));
            Assert.Equal(rsData.Dec2, ls.Dec2);

            Merge(NewMergerS.Prop(_ => _.Date1));
            Assert.Equal(rsData.Date1, ls.Date1);

            Merge(NewMergerS.Prop(_ => _.Date2));
            Assert.Equal(rsData.Date2, ls.Date2);
        }

        [Fact]
        public void MultipleProp()
        {
            Merge(NewMergerS
                .Prop(_ => _.Str1)
                .Prop(_ => _.Str2)
                .Prop(_ => _.Int1)
                .Prop(_ => _.Int2)
                .Prop(_ => _.Dec1)
                .Prop(_ => _.Dec2)
                .Prop(_ => _.Date1)
                .Prop(_ => _.Date2)
            );
            Assert.Equal(rsData.Str1, ls.Str1);
            Assert.Equal(rsData.Str2, ls.Str2);
            Assert.Equal(rsData.Int1, ls.Int1);
            Assert.Equal(rsData.Int2, ls.Int2);
            Assert.Equal(rsData.Dec1, ls.Dec1);
            Assert.Equal(rsData.Dec2, ls.Dec2);
            Assert.Equal(rsData.Date1, ls.Date1);
            Assert.Equal(rsData.Date2, ls.Date2);
        }

        private void Merge(Merger<ModelS> merger)
        {
            Reset();
            merger.Merge(ref ls, rs);
            CheckKey();
        }

        private void Reset()
        {
            lsData = ls = new ModelS {
                Key = "L",
                Str1 = "L.Str1",
                Str2 = "L.Str2",
                Int1 = 101,
                Int2 = null,
                Dec1 = default,
                Dec2 = 1000m,
                Date1 = new DateTime(1001, 1, 1),
                Date2 = new DateTime(1002, 1, 1),
            };
            rsData = rs = new ModelS {
                Key = "R",
                Str1 = "R.Str1",
                Str2 = "R.Str2",
                Int1 = 201,
                Int2 = 202,
                Dec1 = 2001,
                Dec2 = default,
                Date1 = new DateTime(2001, 1, 1),
                Date2 = new DateTime(2002, 1, 1),
            };
        }

        private void CheckKey()
        {
            Assert.Equal(lsData.Key, ls.Key);
        }
    }
}
