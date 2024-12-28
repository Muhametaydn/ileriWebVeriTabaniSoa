using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ileriWebVeriTabaniSoa.Data;
using ileriWebVeriTabaniSoa.Models;
using ileriWebVeriTabaniSoa.Helpers; // Helpers klasöründeki sınıfı dahil ediyoruz
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace ileriWebVeriTabaniSoa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,Password,Role")] User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                // Şifreyi hashle
                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, user.Password);

                // Kullanıcı oluşturulma tarihini UTC olarak ata
                user.CreatedDate = DateTime.UtcNow;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine("asdasd111");
            if (id == null)
            {
                Console.WriteLine("idnull");
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                Console.WriteLine("usernull");
                return NotFound();
            }

            // Şifreyi ve CreatedDate'yi formda göstermiyoruz
            return View(user);
        }


        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,Role")] UserEditDTO user)
        {
            Console.WriteLine("edite girdi.");
            if (id != user.UserId)
            {
                Console.WriteLine("userid Null");
                return NotFound();
            }

            Console.WriteLine("xxxxxx");
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                Console.WriteLine("modelstate valide girdi.");
                try
                {
                    // Veri tabanından mevcut kullanıcıyı getir
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                    if (existingUser == null)
                    {
                        Console.WriteLine("kullaniciyi getir");
                        return NotFound();
                    }

                    // Kullanıcının şifresini değiştirme
                    existingUser.Password = existingUser.Password;  // Eski şifreyi koruyun

                    // Kullanıcı adı ve diğer bilgileri güncelle
                    existingUser.Username = user.Username;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

                    // CreatedDate'yi değiştirmiyoruz
                    existingUser.CreatedDate = existingUser.CreatedDate;

                    // Entity Framework'e değişikliğin olduğunu bildir
                    _context.Entry(existingUser).State = EntityState.Modified;

                    // Değişiklikleri kaydet
                    await _context.SaveChangesAsync();

                    Console.WriteLine("Kullanıcı başarıyla güncellendi.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Eğer ModelState geçerli değilse, hatayı formda göster
            Console.WriteLine("ModelState geçerli değil!");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Hata: {error.ErrorMessage}");
            }

            return View(user);
        }











        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
