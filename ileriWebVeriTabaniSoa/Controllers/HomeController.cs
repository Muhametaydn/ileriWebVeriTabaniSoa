using System.Diagnostics;
using System.Security.Claims;
using ileriWebVeriTabaniSoa.Models;
using Microsoft.AspNetCore.Mvc;
using ileriWebVeriTabaniSoa.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ileriWebVeriTabaniSoa.Helpers;
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





            var posts = _context.Posts.Include(x=>x.Category).ToList(); // Veritaban�ndan t�m verileri al�r
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                Console.WriteLine($"Oturum Bilgisi: Username={username}, Email={email}, Role={role}");
            }
            else
            {
                Console.WriteLine("Kullan�c� giri� yapmam��.");
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
        // Eri�im reddedildi�inde y�nlendirilir
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

            // SearchResults view'�na y�nlendir
            return RedirectToAction("SearchResults");
        }

        public IActionResult SearchResults()
        {
            string searchQuery = TempData["SearchQuery"] as string;

            // Arama kelimesini normalize et
            searchQuery = StringHelper.NormalizeTurkishCharacters(searchQuery).ToLower(); // K���k harfe d�n��t�r

            // Veritaban�ndan t�m postlar� �ek
            var posts = _context.Posts.ToList();

            // Normalize edilmi� ve k���k harfe d�n��t�r�lm�� arama kelimesi ile arama yap
            var filteredPosts = posts.Where(p =>
                StringHelper.NormalizeTurkishCharacters(p.Title).ToLower().Contains(searchQuery) ||
                StringHelper.NormalizeTurkishCharacters(p.Content).ToLower().Contains(searchQuery))
            .ToList();

            ViewBag.SearchQuery = searchQuery;

            return View(filteredPosts);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
