using ileriWebVeriTabaniSoa.Models;
using ileriWebVeriTabaniSoa.Services;
using Microsoft.AspNetCore.Mvc;

namespace ileriWebVeriTabaniSoa.ViewComponents
{
    public class Weather : ViewComponent
    {
        private readonly IWeatherService _weatherService;

        public Weather(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var degree = _weatherService.Degree;
            var description = _weatherService.Description;

            // Debug veya log ekleyebilirsiniz
            Console.WriteLine($"Degree: {degree}, Description: {description}");

            if (degree.HasValue && !string.IsNullOrEmpty(description))
            {
                return View(new WeatherViewModel { Degree = degree, Description = description });
            }
            else
            {
                return Content("Weather data is not available.");
            }
        }
        public class WeatherViewComponent : ViewComponent
        {
            public async Task<IViewComponentResult> InvokeAsync(string city)
            {
                // Hava durumu verilerini al (örnek bir statik veri ile)
                var weatherData = $"Hava durumu {city}: Güneşli, 25°C";

                // Gerekirse bir model döndürün
                return View("Default", weatherData);
            }
        }

    }
}
