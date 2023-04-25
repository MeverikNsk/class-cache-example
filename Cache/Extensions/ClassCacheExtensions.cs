namespace ClassCache.Cache.Extensions
{
    using ClassCache.Cache.Exceptions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ClassCacheExtensions
    {
        public static IServiceCollection AddCacheTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TImplementation>();
            services.AddTransient<CacheProxy<TService, TImplementation>>();

            var opt = services.BuildServiceProvider().GetService<IClassCacheOptions>();
            if (opt == null)
            {
                throw new ClassCacheException("Необходимо зарегистрировать использование ClassCach, выполните метод AddClassCaching");
            }

            return services.AddTransient(proxy => proxy.GetRequiredService<CacheProxy<TService, TImplementation>>().CreateProxy(opt));
        }

        public static IServiceCollection AddClassCaching(this IServiceCollection services, Action<IClassCacheOptions>? options = null)
        {
            services.AddSingleton<IClassCacheOptions>(proxy =>
            {
                var classCacheOptions = new ClassCacheOptions();

                if (options != null)
                {
                    options.Invoke(classCacheOptions);
                }

                return classCacheOptions;
            });

            var opt = services.BuildServiceProvider().GetRequiredService<IClassCacheOptions>();

            if (opt.CachProvider == null)
            {
                services.AddMemoryCache();
                services.AddTransient<ClassCacheProvider>();
            }

            if (opt.CachDurationProvider == null)
            {
                services.AddTransient<ClassCacheDurationProvider>();
            }

            return services;
        }
    }
}
