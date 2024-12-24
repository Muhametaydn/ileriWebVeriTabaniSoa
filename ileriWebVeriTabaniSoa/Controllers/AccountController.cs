using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ileriWebVeriTabaniSoa.Data;
using ileriWebVeriTabaniSoa.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
            // E-posta kontrolü
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                return View(user);
            }

            // Şifreyi hashle
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            // Varsayılan rol atama
            user.Role = "Writer"; // Yeni kayıt olan kullanıcı "Writer" rolü alır
            user.CreatedDate = DateTime.UtcNow;

            // Kullanıcıyı ekle ve kaydet
            _context.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account"); // Başarılı kayıt sonrası Login sayfasına yönlendirin
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
                return View();
            }

            // Şifreyi doğrula
            var passwordHasher = new PasswordHasher<User>(); // User sınıfını kendi modelinize göre değiştirin
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                return View();
            }

            // Oturum açma işlemi
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Kullanıcıyı authenticate et ve cookie'yi set et
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            // Kullanıcıyı çıkart ve cookie'yi temizle
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            //Giriş yaptıktan sonra, kullanıcıya belirli sayfalara erişim izni vermek için [Authorize] özniteliğini (attribute) kullanabilirsiniz. Bu, yalnızca kimliği doğrulanmış kullanıcıların erişebileceği sayfaları korur.
            return View();
        }
    }
}
