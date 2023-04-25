namespace ClassCache.Cache
{
    using Microsoft.Extensions.Caching.StackExchangeRedis;
    using Microsoft.Extensions.Options;

    public class ClassCacheOptions : IOptions<ClassCacheOptions>
    {
        public ICacheProvider? CachProvider { get; set; } = null;

        public ICacheDurationProvider? CachDurationProvider { get; set; } = null;

        public Action<RedisCacheOptions>? RedisCacheOptions { get; private set; } = null;

        public ClassCacheOptions Value => this;
                
        public void UseRedis(Action<RedisCacheOptions> redisSettings)
        {
            RedisCacheOptions = redisSettings;
        }        

    }
}
