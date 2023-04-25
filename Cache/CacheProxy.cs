namespace ClassCache.Cache
{
    using Castle.DynamicProxy;
    using ClassCache.Cache.Redis;
    using Microsoft.Extensions.Options;

    public class CacheProxy<TService, TImplementation>
        where TService : class
        where TImplementation : class, TService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<ClassCacheOptions> _options;

        public CacheProxy(
            IServiceProvider serviceProvider, 
            IOptions<ClassCacheOptions> options) 
        { 
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public TService CreateProxy()
        {
            var generator = new ProxyGenerator();
            var service = _serviceProvider.GetRequiredService<TImplementation>();
            var cachProvider = _options.Value.RedisCacheOptions == null ? _options.Value.CachProvider ?? _serviceProvider.GetRequiredService<ClassCacheProvider>() : _serviceProvider.GetRequiredService<RedisCachProvider>();            
            var durationProvider = _options.Value.CachDurationProvider ?? _serviceProvider.GetRequiredService<ClassCacheDurationProvider>();
            var fooInterfaceProxyWithCallLogerInterceptor = generator.CreateInterfaceProxyWithTarget(
                typeof(TService), 
                service, 
                new CacheInterceptor(
                    cachProvider, 
                    durationProvider));
            return (TService)fooInterfaceProxyWithCallLogerInterceptor;
        }
    }
}
