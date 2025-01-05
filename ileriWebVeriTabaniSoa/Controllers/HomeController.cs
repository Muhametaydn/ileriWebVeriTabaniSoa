using System.Diagnostics;
using System.Security.Claims;
using ileriWebVeriTabaniSoa.Models;
using Microsoft.AspNetCore.Mvc;
using ileriWebVeriTabaniSoa.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ileriWebVeriTabaniSoa.Helpers;
using System.Text;
using System.Text.Json;
using ileriWebVeriTabaniSoa.Services.CurrencyService;

namespace ileriWebVeriTabaniSoa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly CurrencyService _currencyService;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, CurrencyService currencyService)
        {
            _logger = logger;
            _context = context;
            _currencyService = currencyService;
           
        }

        public async Task<IActionResult> Index()
        {


            var dollarRate = await _currencyService.GetDollarRateAsync();
            ViewBag.DollarRate = dollarRate;


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
        public IActionResult Search(string query)
        {
            // Girilen arama kelimesini TempData'ya aktar
            TempData["SearchQuery"] = query;

            // SearchResults view'ýna yönlendir
            return RedirectToAction("SearchResults");
        }

        public IActionResult SearchResults()
        {
            string searchQuery = TempData["SearchQuery"] as string;

            // Arama kelimesi boþsa tüm postlarý göster
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                ViewBag.SearchQuery = string.Empty;
                return View(_context.Posts.ToList());
            }

            // Arama kelimesini normalize et
            searchQuery = StringHelper.NormalizeTurkishCharacters(searchQuery).ToLower(); // Küçük harfe dönüþtür

            // Veritabanýndan tüm postlarý çek
            var posts = _context.Posts.ToList();

            // Normalize edilmiþ ve küçük harfe dönüþtürülmüþ arama kelimesi ile arama yap
            var filteredPosts = posts.Where(p =>
                StringHelper.NormalizeTurkishCharacters(p.Title).ToLower().Contains(searchQuery) ||
                StringHelper.NormalizeTurkishCharacters(p.Content).ToLower().Contains(searchQuery))
            .ToList();

            ViewBag.SearchQuery = searchQuery;

            return View(filteredPosts);
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather([FromBody] String request)
        {
            var url = "http://localhost:4000/send-weather";
            var payload = JsonSerializer.Serialize(new { city = request });
            var client = new HttpClient();
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
