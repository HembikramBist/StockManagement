using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Presentation.Pages.Dashboard
{
    [Authorize(Roles = "Admin,StockManager")]
    public class IndexModel : PageModel
    {
        private readonly StockDbConnect _context;

        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TodaySales { get; set; }
        public int TransactionsToday { get; set; }  //  NEW property

        public IndexModel(StockDbConnect context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            TotalProducts = await _context.Products.CountAsync();
            TotalUsers = await _context.Users.CountAsync();
            TotalQuantity = await _context.Products.SumAsync(p => p.Quantity);

            var today = DateTime.Today;

            // Today's sales amount
            TodaySales = await _context.StockTransactions
                .Where(t => t.SoldAt.Date == today)
                .SumAsync(t => (decimal?)t.GrandTotal) ?? 0;

            // NEW: Count of transactions today
            TransactionsToday = await _context.StockTransactions
                .CountAsync(t => t.SoldAt.Date == today);
        }
    }
}