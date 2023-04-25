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

        public TService CreateProxy()
        {
            var generator = new ProxyGenerator();
            var service = _serviceProvider.GetRequiredService<TImplementation>();
            var cachProvider = _serviceProvider.GetRequiredService<ICacheProvider>();
            var fooInterfaceProxyWithCallLogerInterceptor = generator.CreateInterfaceProxyWithTarget(typeof(TService), service, new CacheInterceptor(cachProvider));
            return (TService)fooInterfaceProxyWithCallLogerInterceptor;
        }
    }
}
