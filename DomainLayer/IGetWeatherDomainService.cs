namespace ClassСache.DomainLayer
{
    using ClassСache.Models;

    public interface IGetWeatherDomainService
    {
        Task<GetWeatherResponse> GetWeatherAsync(GetWeatherRequest request);
    }
}
