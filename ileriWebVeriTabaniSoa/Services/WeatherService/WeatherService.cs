using ileriWebVeriTabaniSoa.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ileriWebVeriTabaniSoa.Services.WeatherService
{
    public class WeatherService : IWeatherService
    {
        private readonly IMemoryCache _memoryCache;
        private const string DegreeKey = "WeatherDegree";
        private const string DescriptionKey = "WeatherDescription";
        public WeatherService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public double? Degree {
            get
            {
                _memoryCache.TryGetValue(DegreeKey, out double? degree);
                return degree;
            }

            set { }
        }
        public string? Description {
            get
            {
                _memoryCache.TryGetValue(DescriptionKey, out string? description);
                return description;
            }

            set { }
        }

        public void update(WeatherModel weatherModel)
        {
            _memoryCache.Set(DegreeKey, weatherModel.Degree);
            _memoryCache.Set(DescriptionKey, weatherModel.Description);
        }
    }
}
