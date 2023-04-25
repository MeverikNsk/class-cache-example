namespace ClassCache.DomainLayer
{
    using ClassCache.Cache;
    using System;
    using System.Collections.Generic;

    public class CustomCachProvider : ICacheProvider
    {
        public void SetValue(IDictionary<string, object> cacheRequest, object? cacheValue, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(IDictionary<string, object> cacheRequest, out object? cacheValue)
        {
            throw new NotImplementedException();
        }
    }
}
