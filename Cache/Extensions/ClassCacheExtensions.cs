namespace ClassCache.Cache.Extensions
{
    using ClassCache.Cache.Exceptions;
    using ClassCache.Cache.Redis;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ClassCacheExtensions
    {
        public static IServiceCollection AddCacheTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TImplementation>();
            services.AddTransient<CacheProxy<TService, TImplementation>>();

            var opt = services.BuildServiceProvider().GetService<ClassCacheOptions>();
            if (opt == null)
            {
                throw new ClassCacheException("Необходимо зарегистрировать использование ClassCach, выполните метод AddClassCaching");
            }

            return services.AddTransient(proxy => proxy.GetRequiredService<CacheProxy<TService, TImplementation>>().CreateProxy());
        }

        public static IServiceCollection AddClassCaching(this IServiceCollection services, Action<ClassCacheOptions>? options = null)
        {
            services.AddTransient<ClassCacheOptions>();
            if (options != null)
            {                
                services.AddOptions();
                services.Configure(options);
            }
                       
            var opt = services.BuildServiceProvider().GetRequiredService<IOptions<ClassCacheOptions>>();

            if (opt.Value.CachProvider == null && opt.Value.RedisCacheOptions == null)
            {
                services.AddMemoryCache();
                services.AddTransient<ClassCacheProvider>();
            }

            if (opt.Value.RedisCacheOptions != null)
            {
                services.AddTransient<RedisCachProvider>();
                services.AddStackExchangeRedisCache(opt.Value.RedisCacheOptions);
            }

            if (opt.Value.CachDurationProvider == null)
            {
                services.AddTransient<ClassCacheDurationProvider>();
            }

            return services;
        }
    }
}
