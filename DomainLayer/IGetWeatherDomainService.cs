namespace ClassCache.DomainLayer
{
    using ClassCache.Models;

    public interface IGetWeatherDomainService
    {
        Task<GetWeatherResponse> GetWeatherAsync(GetWeatherRequest request);

        GetWeatherResponse GetWeather(GetWeatherRequest request);

        Task SetParamAsync(GetWeatherRequest request);

        void SetParam(GetWeatherRequest request);

        Task<GetWeatherResponse> GetWeatherAttributeAsync(GetWeatherRequest request);

        GetWeatherResponse GetWeatherAttribute(GetWeatherRequest request);
    }
}
