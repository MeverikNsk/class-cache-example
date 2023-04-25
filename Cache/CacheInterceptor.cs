namespace ClassCache.Cache
{
    using Castle.DynamicProxy;
    using ClassCache.Cache.Attributes;
    using System.Reflection;

    public class CacheInterceptor : IInterceptor
    {
        private static readonly MethodInfo? handleAsyncMethodInfo = typeof(CacheInterceptor).GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);

        public readonly ICacheProvider _cacheProvider;
        public readonly ICacheDurationProvider _durationProvider;

        public CacheInterceptor(
            ICacheProvider cacheProvider,
            ICacheDurationProvider durationProvider)
        {
            _cacheProvider = cacheProvider;
            _durationProvider = durationProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!CachIsAllow(invocation))
            {
                invocation.Proceed();
                return;
            }

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

                invocation.ReturnValue = result;
                return;
            }

            invocation.Proceed();

            if (invocation.ReturnValue != null)
            {
                if (invocation.ReturnValue is Task task)
                {
                    task.Wait();
                    result = task.GetType().GetProperty("Result")?.GetValue(task);
                    _cacheProvider.SetValue(arguments, result, GetDuration(invocation));
                } 
                else
                {
                    _cacheProvider.SetValue(arguments, invocation.ReturnValue, GetDuration(invocation));
                }
            }

            var value = invocation.ReturnValue;
        }

        private TimeSpan GetDuration(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            return _durationProvider.GetDuration(methodInfo);
        }

        private bool CachIsAllow(IInvocation invocation)
        {
            // Check ignore attribute
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }
            
            if (methodInfo.GetCustomAttributes<IgnoreCachingAttribute>()?.Any() == true)
            {
                return false;
            }

            // Check return type
            var returnType = invocation.Method.ReturnType;

            if (returnType == typeof(void))
            {
                return false;
            }

            if (returnType == typeof(Task) && returnType.GenericTypeArguments?.Any() == false)
            {
                return false;
            }

            return true;
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
    }
}
