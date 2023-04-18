namespace ClassСache.Controllers
{
    using ClassСache.DomainLayer;
    using ClassСache.Models;    
    using Microsoft.AspNetCore.Mvc;    
    
    [ApiController]
    [Route("/api/get-data")]
    public class WeatherController : ControllerBase
    {
        private readonly IGetWeatherDomainService _getWeatherDomainService;
        public WeatherController(IGetWeatherDomainService getWeatherDomainService) 
        { 
            _getWeatherDomainService = getWeatherDomainService;
        }


        [HttpPost]
        [Route("get-weather")]
        public Task<GetWeatherResponse> GetWeatherAsync([FromBody] GetWeatherRequest request)
        {
            return _getWeatherDomainService.GetWeatherAsync(request);
        }
    }
}
