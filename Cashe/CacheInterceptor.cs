namespace ClassСache.Cashe
{
    using Castle.DynamicProxy;
    using System.Reflection;

    public class CacheInterceptor : IInterceptor
    {
        private static readonly MethodInfo? handleAsyncMethodInfo = typeof(CacheInterceptor).GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);

        public ICacheProvider _cacheProvider; 

        public CacheInterceptor(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            var arguments = new Dictionary<string, object>();
            invocation.Method.GetParameters()?.ToList().ForEach(a => arguments.Add(a.Name ?? string.Empty, invocation.GetArgumentValue(a.Position)));

            if (_cacheProvider.TryGetValue(arguments, out object? result))
            {
                var returnType = invocation.Method.ReturnType;

                if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    ExecuteHandleAsyncWithResultUsingReflection(invocation, result);
                    return;
                }                
            }

            invocation.Proceed();

            if (invocation.ReturnValue != null)
            {
                if (invocation.ReturnValue is Task task)
                {
                    task.Wait();
                    result = task.GetType().GetProperty("Result")?.GetValue(task);
                    _cacheProvider.SetValue(arguments, result, new TimeSpan(0, 0, 30));
                }
            }

            var value = invocation.ReturnValue;
        }

        private void ExecuteHandleAsyncWithResultUsingReflection(IInvocation invocation, object? resultValue)
        {
            if (handleAsyncMethodInfo == null)
            {
                throw new InvalidOperationException();
            }

            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = handleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { resultValue });
        }

        private Task<T> HandleAsyncWithResult<T>(T result)
        {
            return Task.FromResult(result);
        }
    }
}
