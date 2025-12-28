using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;
using StockManagement.Presentation.Extensions;

namespace StockManagement.Presentation.Pages.Sell
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly StockDbConnect _context;
        private const string CART_KEY = "SELL_CART";

        public IndexModel(StockDbConnect context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();
        public List<CartItem> Cart { get; set; } = new();

        public decimal GrandTotal => Cart.Sum(c => c.Total);

        //   GET  
        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();
            Cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY)
                   ?? new List<CartItem>();
        }

        //   ADD TO CART  
        public async Task<IActionResult> OnPostAddAsync(int productId)
        {
            Cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY)
                   ?? new List<CartItem>();

            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.Quantity <= 0)
                return RedirectToPage();

            var item = Cart.FirstOrDefault(c => c.ProductId == productId);

            if (item == null)
            {
                Cart.Add(new CartItem
                {
                    ProductId = product.id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            product.Quantity--;

            await _context.SaveChangesAsync();

            HttpContext.Session.SetObject(CART_KEY, Cart);
            return RedirectToPage();
        }

        //   REMOVE FROM CART  
        public async Task<IActionResult> OnPostRemoveAsync(int productId)
        {
            Cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY)
                   ?? new List<CartItem>();

            var item = Cart.FirstOrDefault(c => c.ProductId == productId);
            if (item == null)
                return RedirectToPage();

            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Quantity += item.Quantity;
            }

            Cart.Remove(item);

            await _context.SaveChangesAsync();
            HttpContext.Session.SetObject(CART_KEY, Cart);

            return RedirectToPage();
        }

        //   SAVE TRANSACTION  
        public async Task<IActionResult> OnPostSaveAsync()
        {
            Cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY)
                   ?? new List<CartItem>();

            if (!Cart.Any())
                return RedirectToPage();

            var transaction = new StockTransaction
            {
                UserName = User.Identity?.Name ?? "Unknown",
                SoldAt = DateTime.Now,
                TotalQuantity = Cart.Sum(c => c.Quantity),
                GrandTotal = Cart.Sum(c => c.Total)
            };

            foreach (var item in Cart)
            {
                transaction.Items.Add(new StockTransactionItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.StockTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CART_KEY);
            return RedirectToPage();
        }

       
        public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = "";
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total => Quantity * Price;
        }
    }
}
