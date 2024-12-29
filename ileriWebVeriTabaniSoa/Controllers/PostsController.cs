using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ileriWebVeriTabaniSoa.Data;
using ileriWebVeriTabaniSoa.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ileriWebVeriTabaniSoa.Controllers
{
    [Authorize(Roles = "Admin,Writer")]
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }
        [Route("[controller]")]
        public class PostController : Controller
        {

            [HttpGet]
            public IActionResult Index()
            {
                return View();
            }
        }
        // GET: Posts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Posts.Include(p => p.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var post = _context.Posts.Include(p => p.Category)
                                     .FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Admin,Writer")]
        public async Task<IActionResult> Create()
        {

            Console.WriteLine("buney");

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            var categories = await _context.Categories.ToListAsync();  // Kategorileri veritabanından alıyoruz
            ViewBag.categories = new SelectList(categories, "CategoryID", "Name");  // Kategorileri ViewBag'e atıyoruz
            return View();
        }

        // POST: Posts/Create
        [Authorize(Roles = "Admin,Writer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,UserId,CategoryID")] Post post)
        {

            // Kullanıcının Email veya başka bir kimlik bilgisi üzerinden ID'sini alın
            var currentUser = HttpContext.User;

            var name = currentUser.Identity.Name;


            var user=_context.Users.Where(x => x.Username == name).FirstOrDefault();



             // Kategori kontrolü
             var isValidCategory = _context.Categories.Any(c => c.CategoryID == post.CategoryID);
            if (!isValidCategory)
            {
                Console.WriteLine("Geçersiz kategori seçimi.");
                return BadRequest("Geçersiz kategori seçimi.");
            }

            if (ModelState.IsValid)
            {
                // Kullanıcıyı ve kategoriyi atıyoruz
                // Alınan kullanıcı ID'sini atıyoruz
                post.UserId = user.UserId;

                // Postu veritabanına ekliyoruz
                _context.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendiriyoruz
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("xxxxxxxx");
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            // Hata durumunda kategorileri geri döndürüyoruz
            //ViewData["UserId"] = userId;
            return View(post);
        }


        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId,Category")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", post.UserId);
            return View(post);
        }
        [Authorize(Roles = "Admin")]
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        [Authorize(Roles = "Admin")]
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
