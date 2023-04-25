using ClassCache.Cache;
using ClassCache.DomainLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICacheProvider, CacheProvider>();
builder.Services.AddCacheTransient<IGetWeatherDomainService, GetWeatherDomainService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
