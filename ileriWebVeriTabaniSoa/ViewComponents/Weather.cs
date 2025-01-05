using ileriWebVeriTabaniSoa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ileriWebVeriTabaniSoa.ViewComponents
{
    public class Weather:ViewComponent
    {
        private readonly IWeatherService weatherService;

        public Weather(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.degree=weatherService.Degree;
            ViewBag.description=weatherService.Description;
            return View();
        }

    }
}
