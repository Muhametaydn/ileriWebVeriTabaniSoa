using ileriWebVeriTabaniSoa.Models;

namespace ileriWebVeriTabaniSoa.Services
{
    public interface IWeatherService
    {
        public double? Degree { get; set; }
        public string? Description {  get; set; }

        void update(WeatherModel weatherModel);


    }
}
