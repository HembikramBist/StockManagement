using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Presentation.Pages.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly StockDbConnect _context;

        public IndexModel(StockDbConnect context)
        {
            _context = context;
        }

        public List<TransactionVM> Transactions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;

            // DEFAULT: Show only TODAY's transactions when page first loads
            FromDate ??= today;
            ToDate ??= today;

            // If user picks custom dates, use those instead
            var startDate = FromDate.Value.Date; // 00:00:00
            var endDate = ToDate.Value.Date.AddDays(1).AddTicks(-1); // 23:59:59.999

            IQueryable<StockTransaction> query = _context.StockTransactions
                .Where(t => t.SoldAt >= startDate && t.SoldAt <= endDate);

            Transactions = await query
                .OrderByDescending(t => t.SoldAt)
                .Select(t => new TransactionVM
                {
                    Id = t.id,
                    UserName = t.UserName,
                    TotalQuantity = t.TotalQuantity,
                    GrandTotal = t.GrandTotal,
                    SoldAt = t.SoldAt
                })
                .ToListAsync();
        }

        public class TransactionVM
        {
            public int Id { get; set; }
            public string UserName { get; set; } = "";
            public int TotalQuantity { get; set; }
            public decimal GrandTotal { get; set; }
            public DateTime SoldAt { get; set; }
        }
    }
}