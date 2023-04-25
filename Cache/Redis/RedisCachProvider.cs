namespace ClassCache.Cache.Redis
{
    using ClassCache.Cache.Helpers;
    using Microsoft.Extensions.Caching.Distributed;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    public class RedisCachProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCachProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public void SetValue(IDictionary<string, object> cacheRequest, object? cacheValue, TimeSpan duration)
        {
            var hash = HashHelper.GetHash(cacheRequest);

            if (cacheValue != null)
            {
                var redisVariable = new RedisValue
                {
                    TypeFullName = cacheValue.GetType().FullName ?? throw new Exception($"Ошибка получения полного имени класса объекта {cacheValue}"),
                    JsonValue = JsonSerializer.Serialize(cacheValue)
                };

                var objStr = JsonSerializer.Serialize(redisVariable);                
                _distributedCache.SetString(hash, objStr, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = duration });
            }
        }

        public bool TryGetValue(IDictionary<string, object> cacheRequest, out object? cacheValue)
        {
            var hash = HashHelper.GetHash(cacheRequest);
            var value = _distributedCache.GetString(hash);
            
            if (value != null)
            {
                var objValue = JsonSerializer.Deserialize<RedisValue>(value);
                if (objValue != null)
                {
                    var type = GetTypeByName(objValue.TypeFullName);

                    if (type != null)
                    {
                        cacheValue = (object?)JsonSerializer.Deserialize(objValue.JsonValue, type);
                        return true;
                    }
                }
            }

            cacheValue = null;
            return false;
        }

        private Type? GetTypeByName(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse();

            return assemblies
                        .Select(assembly => assembly.GetType(name))
                        .FirstOrDefault(t => t != null)            
                    ??
                    assemblies                    
                        .SelectMany(assembly => assembly.GetTypes())
                        .FirstOrDefault(t => t.Name.Contains(name));
        }
    }

    public class RedisValue
    {
        public string TypeFullName { get; set; } = string.Empty;

        public string JsonValue { get; set; } = string.Empty;
    }
}
