namespace ClassСache.Cashe
{
    public interface ICacheProvider
    {
        bool TryGetValue(IDictionary<string, object> cacheRequest, out object? cacheValue);

        void SetValue(IDictionary<string, object> cacheRequest, object? cacheValue, TimeSpan duration);
    }
}
