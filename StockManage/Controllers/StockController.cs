using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Presentation.Controllers
{
    [Authorize(Roles = "Admin,StockManager")]
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly StockDbConnect _context;

        public StockController(StockDbConnect context)
        {
            _context = context;
        }

        // POST: api/stock/in?productId=1&quantity=50
        [HttpPost("in")]
        public async Task<IActionResult> StockIn(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            if (quantity <= 0)
                return BadRequest("Quantity must be positive");

            product.Quantity += quantity;

            var transaction = new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                Type = "IN",
                CreatedAt = DateTime.UtcNow
            };

            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok("Stock added successfully");
        }

        // POST: api/stock/out?productId=1&quantity=20
        [HttpPost("out")]
        public async Task<IActionResult> StockOut(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            if (quantity <= 0)
                return BadRequest("Quantity must be positive");

            if (product.Quantity < quantity)
                return BadRequest("Not enough stock");

            product.Quantity -= quantity;

            var transaction = new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                Type = "OUT",
                CreatedAt = DateTime.UtcNow
            };

            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok("Stock removed successfully");
        }
    }
}
