using System;
using System.Linq;

using Castle.DynamicProxy;

namespace Application
{
    using DataAccessor;

    public class Accessor
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public static T Create<T>()
            where T : class, new()
        {
            var intercepter = new Intercepter();
            var proxy = generator.CreateClassProxy<T>(intercepter);

            return proxy;
        }

        private class Intercepter : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                var typeName = invocation.TargetType.Name;
                var methodName = invocation.Method.Name;
                var isSelect = methodName == "Select";
                var isDelete = methodName == "Delete";

                ShowLog($"{typeName}.{methodName}({ArgsToString(invocation.Arguments)})");

                switch (typeName)
                {
                    case nameof(Owner):
                        InterceptOwner(invocation, isSelect, isDelete);
                        break;

                    case nameof(Lion):
                        InterceptLion(invocation, isSelect, isDelete);
                        break;

                    default:
                        invocation.Proceed();
                        break;
                }
            }

            public void InterceptOwner(IInvocation invocation, bool isSelect, bool isDelete)
            {
                if (isDelete)
                {
                    var allowed = new[] { 87 };
                    var ids = invocation.Arguments[0] as int[];
                    if (ids.Except(new[] { 87 }).Any())
                    {
                        ShowInfo($"You can only delete '{IntsToString(allowed)}'!");
                        return;
                    }
                }

                invocation.Proceed();
            }

            public void InterceptLion(IInvocation invocation, bool isSelect, bool isDelete)
            {
                if (isDelete)
                {
                    ShowInfo("I LIKE LIONS, you CAN NOT delete any lions!");
                    return;
                }

                if (isSelect)
                {
                    var favoriteId = 1;
                    ShowInfo($"You CAN ONLY get my favorite lion with id '{favoriteId}'!");

                    invocation.Proceed();
                    invocation.ReturnValue = new[] { 1 };
                }
            }

            private string ArgsToString(object[] args) =>
                string.Join(", ", args.Select(a =>
                    a is int[] ? IntsToString(a as int[]) :
                    a is string ? $"\"{a}\"" :
                    a.ToString()));

            private string IntsToString(int[] vs) =>
                string.Join(", ", vs);

            private void ShowInfo(string message) =>
                Console.WriteLine($"[ Proxy ] {message}");

            private void ShowLog(string message) =>
                Console.WriteLine($"[  LOG  ] {message}");
        }
    }
}
