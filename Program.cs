using ClassCache.Cache.Extensions;
using ClassCache.DomainLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddClassCaching(opt =>
{
    opt.CachProvider = new CustomCachProvider();
        
    opt.UseRedis(settings =>
    {
        settings.Configuration = "localhost";
        settings.InstanceName = "classCachLocal";
    });
});

builder.Services.AddCacheTransient<IGetWeatherDomainService, GetWeatherDomainService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
