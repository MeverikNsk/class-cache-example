namespace ClassCache.Cache
{
    using ClassCache.Cache.Helpers;
    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Generic;

    public class ClassCacheProvider : ICacheProvider
    {
        public IMemoryCache _memoryCache;

        public ClassCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool TryGetValue(IDictionary<string, object> cacheRequest, out object? cacheValue)
        {                        
            var hash = HashHelper.GetHash(cacheRequest);
            return _memoryCache.TryGetValue(hash, out cacheValue);
        }

        public void SetValue(IDictionary<string, object> cacheRequest, object? cacheValue, TimeSpan duration)
        {
            var hash = HashHelper.GetHash(cacheRequest);
            _memoryCache.Set(hash, cacheValue, duration);
        }
    }
}
