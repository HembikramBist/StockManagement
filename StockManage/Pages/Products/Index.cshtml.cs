using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Presentation.Pages.Products
{
    [Authorize] 
    public class IndexModel : PageModel
    {
        private readonly StockDbConnect _context;

        public IndexModel(StockDbConnect context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty]
        public Product NewProduct { get; set; } = new();

        [BindProperty]
        public Product EditProduct { get; set; } = new();

        public async Task OnGetAsync()
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrWhiteSpace(Search))
                query = query.Where(p => p.ProductName.Contains(Search));

            Products = await query.ToListAsync();
        }

        // ADD
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Products.Add(NewProduct);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        // EDIT
        [Authorize(Roles = "Admin,StockManager")]
        public async Task<IActionResult> OnPostEditAsync()
        {
            _context.Products.Update(EditProduct);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        // DELETE
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
