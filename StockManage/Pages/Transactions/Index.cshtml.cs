using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task OnGetAsync()
        {
            Transactions = await _context.StockTransactions
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
