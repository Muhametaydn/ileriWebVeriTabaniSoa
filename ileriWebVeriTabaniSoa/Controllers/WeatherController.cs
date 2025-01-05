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
            

            _weatherService.update(weatherModel);
            return Ok(new { message = "Rate received successfully." });
        }
    }
}
