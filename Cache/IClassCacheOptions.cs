namespace ClassCache.Cache
{
    public interface IClassCacheOptions
    {
        ICacheProvider? CachProvider { get; set; }

        ICacheDurationProvider? CachDurationProvider { get; set; }
    }
}
