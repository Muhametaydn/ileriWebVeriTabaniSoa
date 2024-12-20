using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ileriWebVeriTabaniSoa.Data;
using ileriWebVeriTabaniSoa.Models;
using ileriWebVeriTabaniSoa.Helpers;

namespace ileriWebVeriTabaniSoa.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Email,Password,Role")] User user)
        {

            if (true)
            {
                Console.WriteLine("modele girdi");
                // E-posta kontrolü
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                    Console.WriteLine("Bu e-posta adresi zaten kullanılıyor");
                    return View(user);
                }

                // Şifreyi hashle
                user.Password = PasswordHasher.HashPassword(user.Password);

                // Varsayılan rol atama
                user.Role = "Writer"; // Yeni kayıt olan kullanıcı "Writer" rolü alır
                user.CreatedDate = DateTime.UtcNow;

                // Kullanıcıyı ekle ve kaydet
                _context.Add(user);
                await _context.SaveChangesAsync();
                Console.WriteLine("Register oldu11");
                return RedirectToAction("Login", "Account"); // Başarılı kayıt sonrası Login sayfasına yönlendirin
                Console.WriteLine("Register oldu2");
            }
            Console.WriteLine("register butonuna bastim");
            return View(user);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            // Kullanıcıyı e-posta adresi ile bul
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                Console.WriteLine("E-posta veya şifre yanlış");
                return View();
            }

            // Şifreyi doğrula
            var hashedPassword = PasswordHasher.HashPassword(password);
            if (user.Password != hashedPassword)
            {
                ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                Console.WriteLine("E-posta veya şifre yanlış222");
                return View();
            }

            // Oturum açma işlemi (örneğin, kullanıcı bilgilerini Session'da saklama)
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            // Başarılı giriş sonrası yönlendirme
            Console.WriteLine("Login oldu");
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            // Oturum kapatma işlemi buraya
            TempData["Message"] = "Oturum kapatıldı.";
            return RedirectToAction("Login");
        }
    }
}