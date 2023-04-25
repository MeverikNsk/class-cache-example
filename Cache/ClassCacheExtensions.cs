namespace ClassCache.Cache
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ClassCacheExtensions
    {
        public static IServiceCollection AddCacheTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TImplementation>();
            services.AddTransient<CacheProxy<TService, TImplementation>>();
            return services.AddTransient(proxy => proxy.GetRequiredService<CacheProxy<TService, TImplementation>>().CreateProxy());
        }        
    }
}
