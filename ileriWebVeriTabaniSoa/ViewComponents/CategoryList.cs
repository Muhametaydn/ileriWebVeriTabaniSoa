using ileriWebVeriTabaniSoa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ileriWebVeriTabaniSoa.ViewComponents
{
    public class CategoryList : ViewComponent
    {
        
        private readonly AppDbContext _context;

        public CategoryList(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("No categories found in the database.");
            }
            else
            {
                Console.WriteLine($"Found {categories.Count} categories.");
            }
            return View(categories);
        }
    }
}
