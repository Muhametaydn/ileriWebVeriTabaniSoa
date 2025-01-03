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

namespace ileriWebVeriTabaniSoa.Controllers
{
    [Authorize(Roles = "Admin,Writer")]
    public class LikesController : Controller
    {
        private readonly AppDbContext _context;

        public LikesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Likes
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Likes.Include(l => l.Post).Include(l => l.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Likes/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Post)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeID == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        [Authorize(Roles = "Admin,Writer")]
        public IActionResult Create()
        {
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Likes/Create
        // POST: Likes/Create
    [Authorize(Roles = "Admin,Writer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LikeID,PostID,UserID,LikedDate")] Like like)
    {
    // Kullanıcıyı al
    var currentUser = HttpContext.User;
    var name = currentUser.Identity.Name;
    var user = _context.Users.Where(x => x.Username == name).FirstOrDefault();

    // Kullanıcının bu gönderiyi daha önce beğenip beğenmediğini kontrol et
    var existingLike = _context.Likes
        .FirstOrDefault(l => l.PostID == like.PostID && l.UserID == user.UserId);

    if (existingLike != null)
    {
        // Kullanıcı zaten beğenmiş, işlemi iptal et
        return RedirectToAction("Details", "Posts", new { id = like.PostID });
    }

    // Kullanıcı daha önce beğenmemişse, beğeni ekle
    like.UserID = user.UserId;
    _context.Add(like);
    await _context.SaveChangesAsync();

    // Beğeni ekledikten sonra Post detay sayfasına yönlendir
    return RedirectToAction("Details", "Posts", new { id = like.PostID });
}
        // GET: Likes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id", like.PostID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", like.UserID);
            return View(like);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikeID,PostID,UserID,LikedDate")] Like like)
        {
            if (id != like.LikeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.LikeID))
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
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id", like.PostID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", like.UserID);
            return View(like);
        }

        // GET: Likes/Delete/5
        [Authorize(Roles = "Admin,Writer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Post)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeID == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Likes/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like != null)
            {
                _context.Likes.Remove(like);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Writer")]
        [HttpPost]
        public IActionResult LikeDelete(int PostID)
        {
            var currentUser = HttpContext.User;  // Kullanıcı bilgilerini al
            var name = currentUser.Identity.Name;  // Kullanıcının adını al
            var user = _context.Users.Where(x => x.Username == name).FirstOrDefault();  // Kullanıcıyı veritabanında ara

            if (user != null)
            {
                var like = _context.Likes
                                   .FirstOrDefault(l => l.PostID == PostID && l.UserID == user.UserId);  // Kullanıcıya ait beğeniyi bul

                if (like != null)
                {
                    _context.Likes.Remove(like);  // Beğeniyi sil
                    _context.SaveChanges();  // Değişiklikleri kaydet
                }
            }

            return RedirectToAction("Details", "Posts", new { id = PostID });  // Kullanıcıyı ilgili sayfaya yönlendir
        }


        [Authorize(Roles = "Admin,Writer")]
        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.LikeID == id);
        }
    }
}
