﻿namespace ClassCache.DomainLayer
{
    using ClassCache.Cache.Attributes;
    using ClassCache.Models;
    using System.Threading.Tasks;

    public class GetWeatherDomainService : IGetWeatherDomainService
    {
        private readonly ILogger<GetWeatherDomainService> _logger;
        public GetWeatherDomainService(ILogger<GetWeatherDomainService> logger)
        {
            _logger = logger;
        }

        [CacheDuration(0, 1, 48)]
        public async Task<GetWeatherResponse> GetWeatherAsync(GetWeatherRequest request)
        {
            return await Task.FromResult(GetWeather(request));
        }

        [CacheDuration(0, 1, 48)]
        public GetWeatherResponse GetWeather(GetWeatherRequest request)
        {

            Thread.Sleep(3000);

            var result = new GetWeatherResponse
            {
                Items = new List<string>
                {
                }
            };

            var rand = new Random();
            var count = rand.Next(1, request.Id);

            for (int i = 0; i < count; i++)
            {
                result.Items.Add($"line {i}");
            }

            return result;
        }

        public async Task SetParamAsync(GetWeatherRequest request)
        {
            await Task.Run(() => { SetParam(request); });
        }

        public void SetParam(GetWeatherRequest request)
        {            
        }

        [IgnoreCaching]
        public async Task<GetWeatherResponse> GetWeatherAttributeAsync(GetWeatherRequest request)
        {
            return await Task.FromResult(GetWeatherAttribute(request));
        }

        [IgnoreCaching]
        public GetWeatherResponse GetWeatherAttribute(GetWeatherRequest request)
        {
            return GetWeather(request);
        }
    }
}
