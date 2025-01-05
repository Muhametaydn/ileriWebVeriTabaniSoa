using ileriWebVeriTabaniSoa.Models;
using ileriWebVeriTabaniSoa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ileriWebVeriTabaniSoa.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpPost("receive")]
        public IActionResult Receive([FromBody] WeatherModel weatherModel)
        {
            if (weatherModel == null)
            {
                return BadRequest("Invalid weather data.");
            }

            // Verinin alındığını logluyoruz (debug için)
            Console.WriteLine($"Weather received: Degree = {weatherModel.Degree}, Description = {weatherModel.Description}");

            // WeatherService'e veri gönderiliyor
            _weatherService.Update(weatherModel);

            return Ok(new { message = "Rate received successfully." });
        }

        [HttpGet("current")]
        public IActionResult GetCurrentWeather()
        {
            var degree = _weatherService.Degree;
            var description = _weatherService.Description;

            // Hava durumu verisi varsa, başarılı döndürüyoruz
            if (degree.HasValue && !string.IsNullOrEmpty(description))
            {
                return Ok(new { degree, description });
            }
            else
            {
                return NotFound("Weather data is not available.");
            }
        }
    }
}
