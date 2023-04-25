namespace ClassCache.Cache
{
    public class ClassCacheOptions : IClassCacheOptions
    {
        public ICacheProvider? CachProvider { get; set; } = null;

        public ICacheDurationProvider? CachDurationProvider { get; set; } = null;
    }
}
