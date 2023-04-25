namespace ClassCache.Cache
{
    using System;
    using System.Reflection;
    using ClassCache.Cache.Attributes;

    public class ClassCacheDurationProvider : ICacheDurationProvider
    {
        public TimeSpan GetDuration(MethodInfo methodInfo)
        {
            var durationAttribute = methodInfo.GetCustomAttribute<CacheDurationAttribute>();

            if (durationAttribute != null)
            {
                return durationAttribute.Duration;
            }

            return new TimeSpan();
        }
    }
}
