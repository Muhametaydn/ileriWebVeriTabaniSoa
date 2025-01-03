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
    public class CommentsController : Controller
    {

        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Comments.Include(c => c.Post).Include(c => c.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
        // GET: Comments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Comments/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentID,Content,PostID,UserID")] Comment comment)
        {
            // Kullanıcının Email veya başka bir kimlik bilgisi üzerinden ID'sini alın
            var currentUser = HttpContext.User;

            var name = currentUser.Identity.Name;


            var user = _context.Users.Where(x => x.Username == name).FirstOrDefault();
            
                Console.Write("modelstateCommente Create girdi");
                // Yorum veritabanına ekleniyor
                comment.UserID = user.UserId;
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Yorum eklenip kaydedildikten sonra aynı Post detay sayfasına yönlendir
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            
            //bundan sonrasi calismiyor
            // Model geçerli değilse, formu tekrar göster
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id", comment.PostID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", comment.UserID);
            Console.Write("yorum yapildi");
            return RedirectToAction("Index", "Home");
        }



        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id", comment.PostID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", comment.UserID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentID,Content,PostID,UserID,CreatedDate")] Comment comment)
        {
            if (id != comment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentID))
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
            ViewData["PostID"] = new SelectList(_context.Posts, "Id", "Id", comment.PostID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentID == id);
        }
    }
}
