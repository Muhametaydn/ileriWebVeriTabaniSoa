using System.Diagnostics;
using System.Security.Claims;
using ileriWebVeriTabaniSoa.Models;
using Microsoft.AspNetCore.Mvc;
using ileriWebVeriTabaniSoa.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace ileriWebVeriTabaniSoa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {





            var posts = _context.Posts.Include(x=>x.Category).ToList(); // Veritabanýndan tüm verileri alýr
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                Console.WriteLine($"Oturum Bilgisi: Username={username}, Email={email}, Role={role}");
            }
            else
            {
                Console.WriteLine("Kullanýcý giriþ yapmamýþ.");
            }

            return View(posts);
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
        // Eriþim reddedildiðinde yönlendirilir
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult CreatePost()
        {
            return View();
        }
        public IActionResult GetCategory()
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
