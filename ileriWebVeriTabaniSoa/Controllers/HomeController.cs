using System.Diagnostics;
using ileriWebVeriTabaniSoa.Models;
using Microsoft.AspNetCore.Mvc;

namespace ileriWebVeriTabaniSoa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //burada login olmussa consolda bilgileri yazdiriyor guvenlik icin sonda sil
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            // Eðer kullanýcý giriþi yapýlmýþsa, bilgileri gösterin.
            Console.WriteLine($"Oturum Bilgisi: UserId={userId}, Username={username}, Role={role}");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
