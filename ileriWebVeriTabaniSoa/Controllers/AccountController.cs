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
                ModelState.AddModelError("Email", "This email address is already in use.");
                return View(user);
            }

            // Kullanıcı adı kontrolü
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "This username is already in use.");
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
        //AccessDenied = Bu sayfada kullanıcıya bir hata mesajı gösterebilirsiniz: "Bu sayfaya erişim izniniz yok."
        public IActionResult AccessDenied()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or password incorrect.");
                ViewData["ErrorMessage"] = "Email or password incorrect!";
                Console.Write("E-posta veya şifre yanlış.");
                return View();
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Email or password incorrect.");
                ViewData["ErrorMessage"] = "Email or password incorrect!";
                Console.Write("E-posta veya şifre yanlış.2");
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            Console.Write("giris tamamlandi");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Console.Write("cikis yapildi");
            return RedirectToAction("Login");
        }



        [Authorize]
        public IActionResult Dashboard()
        {
            //Giriş yaptıktan sonra, kullanıcıya belirli sayfalara erişim izni vermek için [Authorize] özniteliğini (attribute) kullanabilirsiniz. Bu, yalnızca kimliği doğrulanmış kullanıcıların erişebileceği sayfaları korur.
            Console.WriteLine("bura ne alaka");
            return View();
        }
    }
}
