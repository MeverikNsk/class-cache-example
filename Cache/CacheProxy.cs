namespace ClassCache.Cache
{
    using Castle.DynamicProxy;

    public class CacheProxy<TService, TImplementation>
        where TService : class
        where TImplementation : class, TService
    {
        private readonly IServiceProvider _serviceProvider;

        public CacheProxy(IServiceProvider serviceProvider) 
        { 
            _serviceProvider = serviceProvider;
        }

        public TService CreateProxy(IClassCacheOptions options)
        {
            var generator = new ProxyGenerator();
            var service = _serviceProvider.GetRequiredService<TImplementation>();
            var cachProvider = options.CachProvider ?? _serviceProvider.GetRequiredService<ClassCacheProvider>();
            var durationProvider = options.CachDurationProvider ?? _serviceProvider.GetRequiredService<ClassCacheDurationProvider>();
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
