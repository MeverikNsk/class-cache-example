namespace ClassСache.DomainLayer
{
    using ClassСache.Models;
    using System.Threading.Tasks;

    public class GetWeatherDomainService : IGetWeatherDomainService
    {
        private readonly ILogger<GetWeatherDomainService> _logger;
        public GetWeatherDomainService(ILogger<GetWeatherDomainService> logger)
        {
            _logger = logger;
        }

        public async Task<GetWeatherResponse> GetWeatherAsync(GetWeatherRequest request)
        {
            var result = new GetWeatherResponse
            {
                Items = new List<string>
                {
                }
            };

            var rand = new Random();
            var count = rand.Next(1, request.Id);

            for(int i = 0; i < count; i++)
            {
                result.Items.Add($"line {i}");
            }

            return await Task.FromResult(result);
        }
    }
}
