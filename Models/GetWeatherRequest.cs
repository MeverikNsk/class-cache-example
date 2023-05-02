namespace ClassCache.Models
{
    using ClassCache.Cache;

    public class GetWeatherRequest : IClassCachHash
    {
        public int Id { get; set; }

        public string GetHash()
        {
            return Id.ToString();
        }
    }
}
