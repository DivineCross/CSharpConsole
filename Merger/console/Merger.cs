using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApplication
{
    public partial class Merger<T>
        where T : new()
    {
        public static Merger<T> Create()
        {
            return new Merger<T> {
                Actions = new List<Action>()
            };
        }

        public T Merge(T l, T r)
        {
            if (l == null || r == null)
                return l;

            return Merge(ref l , r);
        }

        public T Merge(ref T l, T r)
        {
            if (r == null)
            {
                l = default;
                return l;
            }
            if (l == null)
            {
                l = new T();
            }

            L = l;
            R = r;

            Actions.ForEach(x => x.Invoke());

            return l;
        }

        public Merger<T> Prop<TProp>(Expression<Func<T, TProp>> expr)
        {
            Actions.Add(GetSetAction(expr));

            return this;
        }

        public Merger<T> Prop<TProp>(Expression<Func<T, TProp>> expr, Merger<TProp> merger)
            where TProp : new()
        {
            var setter = GetSetter(expr);

            return AddAction(() => {
                var propL = GetValue(expr, L);
                var propR = GetValue(expr, R);
                var merged = merger.Merge(ref propL, propR);

                setter.Invoke(L, merged);
            });
        }

        public Merger<T> PropInner<TProp, TItem>(
            Expression<Func<T, TProp>> expr,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            return PropInner<TProp, TItem, int>(expr, null, merger);
        }

        public Merger<T> PropInner<TProp, TItem, TKey>(
            Expression<Func<T, TProp>> expr,
            Func<TItem, TKey> keySelector,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            var setter = GetSetter(expr);

            return AddAction(() => {
                var ls = GetValue(expr, L);
                var rs = GetValue(expr, R);

                if (ls?.Any() != true)
                    return;

                if (rs == null)
                {
                    setter.Invoke(L, default(TProp));
                    return;
                }
                if (!rs.Any())
                {
                    ls.Clear();
                    return;
                }

                MergeList(ls, rs, keySelector, merger, MergeInner);
                setter.Invoke(L, ls);
            });
        }

        public Merger<T> PropLeft<TProp, TItem>(
            Expression<Func<T, TProp>> expr,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            return PropLeft<TProp, TItem, int>(expr, null, merger);
        }

        public Merger<T> PropLeft<TProp, TItem, TKey>(
            Expression<Func<T, TProp>> expr,
            Func<TItem, TKey> keySelector,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            var setter = GetSetter(expr);

            return AddAction(() => {
                var ls = GetValue(expr, L);
                var rs = GetValue(expr, R);

                if (ls?.Any() != true) return;
                if (rs?.Any() != true) return;

                MergeList(ls, rs, keySelector, merger, MergeLeft);
                setter.Invoke(L, ls);
            });
        }

        public Merger<T> PropRight<TProp, TItem>(
            Expression<Func<T, TProp>> expr,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            return PropRight<TProp, TItem, int>(expr, null, merger);
        }

        public Merger<T> PropRight<TProp, TItem, TKey>(
            Expression<Func<T, TProp>> expr,
            Func<TItem, TKey> keySelector,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            var setter = GetSetter(expr);

            return AddAction(() => {
                var ls = GetValue(expr, L);
                var rs = GetValue(expr, R);

                if (rs == null)
                {
                    setter.Invoke(L, default(TProp));
                    return;
                }
                if (!rs.Any())
                {
                    ls.Clear();
                    return;
                }

                if (ls == null)
                    ls = new TProp();

                MergeList(ls, rs, keySelector, merger, MergeRight);
                setter.Invoke(L, ls);
            });
        }

        public Merger<T> PropFull<TProp, TItem>(
            Expression<Func<T, TProp>> expr,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            return PropFull<TProp, TItem, int>(expr, null, merger);
        }

        public Merger<T> PropFull<TProp, TItem, TKey>(
            Expression<Func<T, TProp>> expr,
            Func<TItem, TKey> keySelector,
            Merger<TItem> merger)
            where TProp : IList<TItem>, new()
            where TItem : new()
        {
            var setter = GetSetter(expr);

            return AddAction(() => {
                var ls = GetValue(expr, L);
                var rs = GetValue(expr, R);

                if (rs?.Any() != true)
                    return;

                if (ls == null)
                    ls = new TProp();

                MergeList(ls, rs, keySelector, merger, MergeFull);
                setter.Invoke(L, ls);
            });
        }
    }

    public partial class Merger<T>
    {
        private void MergeList<TKey, TValue>(
            IList<TValue> ls,
            IList<TValue> rs,
            Func<TValue, TKey> keySelector,
            Merger<TValue> merger,
            Func<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, Merger<TValue>, IList<TValue>> impl)
            where TValue : new()
        {
            var lmap = getMap(ls);
            var rmap = getMap(rs);
            var values = impl.Invoke(lmap, rmap, merger);

            ls.Clear();
            foreach (var v in values)
                ls.Add(v);

            Dictionary<TKey, TValue> getMap(IList<TValue> list) =>
                keySelector != null
                    ? list.ToDictionary(keySelector)
                    : list.Select((v, i) => (v, i)).ToDictionary(t => t.i, t => t.v) as Dictionary<TKey, TValue>;
        }

        private IList<TValue> MergeInner<TKey, TValue>(
            Dictionary<TKey, TValue> ls,
            Dictionary<TKey, TValue> rs,
            Merger<TValue> merger)
            where TValue : new()
        {
            var joins = ls.Join(rs, l => l.Key, r => r.Key, (l, r) => (k: l.Key, l: l.Value, r: r.Value));

            return joins.Select(p => {
                merger.Merge(ref p.l, p.r);

                return p.l;
            }).ToList();
        }

        private IList<TValue> MergeLeft<TKey, TValue>(
            Dictionary<TKey, TValue> ls,
            Dictionary<TKey, TValue> rs,
            Merger<TValue> merger)
            where TValue : new()
        {
            return ls.Select(kv => {
                var k = kv.Key;
                var lv = kv.Value;

                if (rs.ContainsKey(k))
                    merger.Merge(ref lv, rs[k]);

                return lv;
            }).ToList();
        }

        private IList<TValue> MergeRight<TKey, TValue>(
            Dictionary<TKey, TValue> ls,
            Dictionary<TKey, TValue> rs,
            Merger<TValue> merger)
            where TValue : new()
        {
            return rs.Select(kv => {
                var k = kv.Key;
                var rv = kv.Value;

                var lv = ls.ContainsKey(k) ? ls[k] : new TValue();
                merger.Merge(ref lv, rv);

                return lv;
            }).ToList();
        }

        private IList<TValue> MergeFull<TKey, TValue>(
            Dictionary<TKey, TValue> ls,
            Dictionary<TKey, TValue> rs,
            Merger<TValue> merger)
            where TValue : new()
        {
            var rkeys = new HashSet<TKey>(rs.Keys);

            var vs = ls.Select(kv => {
                var k = kv.Key;
                var lv = kv.Value;

                if (rs.ContainsKey(k))
                {
                    merger.Merge(ref lv, rs[k]);
                    rkeys.Remove(k);
                }

                return lv;
            }).ToList();

            var remained = rs.Where(r => rkeys.Contains(r.Key));
            vs.AddRange(remained.Select(kv => {
                var lv = new TValue();
                merger.Merge(ref lv, kv.Value);

                return lv;
            }).ToList());

            return vs;
        }
    }

    public partial class Merger<T>
    {
        private T L { get; set; }

        private T R { get; set; }

        private List<Action> Actions { get; set; }

        private Action GetSetAction<TProp>(Expression<Func<T, TProp>> expr)
        {
            var setL = ExpressionHelper.CreateSetter(expr);
            Func<TProp> getR = () => ExpressionHelper.GetValue(expr, R);

            return () => setL(L, getR());
        }

        private Action<T, TProp> GetSetter<TProp>(Expression<Func<T, TProp>> expr)
        {
            return ExpressionHelper.CreateSetter(expr);
        }

        private TProp GetValue<TProp>(Expression<Func<T, TProp>> expr, T m)
        {
            return ExpressionHelper.GetValue(expr, m);
        }

        private Merger<T> AddAction(Action a)
        {
            Actions.Add(a);

            return this;
        }
    }
}
