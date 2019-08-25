using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApplication
{
    public static class ExpressionHelper
    {
        public static Action<TModel, TProp> CreateSetter<TModel, TProp>(Expression<Func<TModel, TProp>> expr)
        {
            // Support only the lambda expression of the form 'x => x.Member'.

            // x => 'x.Member'
            var memberExpr = expr.Body as MemberExpression;
            // 'x' => x.Member
            var paramExpr = expr.Parameters[0];

            if (memberExpr == null) return null;

            // 'x'.Member
            var memberExprExpr = memberExpr.Expression;

            if (paramExpr != memberExprExpr) return null;

            // Make '(x, v) => x.Member = v'.

            // ('x', v) => x.Member = v
            var paramX = paramExpr;
            // (x, 'v') => x.Member = v
            var paramV = Expression.Parameter(typeof(TProp));

            // (x, v) => 'x.Member' = v
            var left = memberExpr;
            // (x, v) => x.Member = 'v'
            var right = paramV;
            // (x, v) => 'x.Member = v'
            var assign = Expression.Assign(left, right);

            // '(x, v) => x.Member = v'
            var lambda = Expression.Lambda<Action<TModel, TProp>>(assign, paramX, paramV);
            var compiled = lambda.Compile();

            return compiled;
        }

        public static TProp GetValue<TModel, TProp>(Expression<Func<TModel, TProp>> expr, TModel m)
        {
            return Compiler<TModel, TProp>.Compile(expr).Invoke(m);
        }

        private static class Compiler<TIn, TOut>
        {
            private static ConcurrentDictionary<MemberInfo, Func<TIn, TOut>> simpleMemberAccessMap =
                new ConcurrentDictionary<MemberInfo, Func<TIn, TOut>>();

            public static Func<TIn, TOut> Compile(Expression<Func<TIn, TOut>> expr)
            {
                return CompileFromMemberAccess(expr)
                    ?? CompileSlow(expr);
            }

            private static Func<TIn, TOut> CompileFromMemberAccess(Expression<Func<TIn, TOut>> expr)
            {
                // Support only the lambda expression of the form 'x => x.Member'.

                // x => 'x.Member'
                var memberExpr = expr.Body as MemberExpression;
                // 'x' => x.Member
                var paramExpr = expr.Parameters[0];

                if (memberExpr == null) return null;

                // 'x'.Member
                var memberExprExpr = memberExpr.Expression;
                // x.'Member'
                var member = memberExpr.Member;

                if (paramExpr != memberExprExpr) return null;

                return simpleMemberAccessMap.GetOrAdd(member, _ => CompileSlow(expr));
            }

            private static Func<TIn, TOut> CompileSlow(Expression<Func<TIn, TOut>> expr)
            {
                return expr.Compile();
            }
        }
    }
}
