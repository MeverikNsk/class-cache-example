namespace ClassCache.Controllers
{
    using ClassCache.DomainLayer;
    using ClassCache.Models;    
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
        [Route("get-weather-async")]
        public Task<GetWeatherResponse> GetWeatherAsync([FromBody] GetWeatherRequest request)
        {
            return _getWeatherDomainService.GetWeatherAsync(request);
        }

        [HttpPost]
        [Route("get-weather")]
        public GetWeatherResponse GetWeather([FromBody] GetWeatherRequest request)
        {
            return _getWeatherDomainService.GetWeather(request);
        }

        [HttpPost]
        [Route("set-param-async")]
        public Task SetParamAsync([FromBody] GetWeatherRequest request)
        {
            return _getWeatherDomainService.SetParamAsync(request);
        }

        [HttpPost]
        [Route("set-param")]
        public void SetParam([FromBody] GetWeatherRequest request)
        {
            _getWeatherDomainService.SetParam(request);
        }

        [HttpPost]
        [Route("get-weather-attr-async")]
        public Task<GetWeatherResponse> GetWeatherAttribureAsync([FromBody] GetWeatherRequest request)
        {
            return _getWeatherDomainService.GetWeatherAttributeAsync(request);
        }

        [HttpPost]
        [Route("get-weather-attr")]
        public GetWeatherResponse GetWeatherAttribute([FromBody] GetWeatherRequest request)
        {
            return _getWeatherDomainService.GetWeatherAttribute(request);
        }
    }
}
