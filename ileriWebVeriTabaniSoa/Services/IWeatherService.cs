using ileriWebVeriTabaniSoa.Models;

namespace ileriWebVeriTabaniSoa.Services
{
    public interface IWeatherService
    {
        double? Degree { get; }
        string? Description { get; }

        void Update(WeatherModel weatherModel);  // Güncelleme için sadece method kullanılmalı
    }
}
