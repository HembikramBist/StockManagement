using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly StockDbConnect _context;

        public ProductsController(StockDbConnect context)
        {
            _context = context;
        }

        // POST: api/products
        [HttpPost]
        [Authorize(Roles = "Admin,StockManager")]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        // GET: api/products
        [HttpGet]
        [Authorize(Roles = "Admin,StockManager,Viewer")]
        public IActionResult GetAll()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,StockManager")]
        public IActionResult Update(int id, Product updatedProduct)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            product.ProductName = updatedProduct.ProductName;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;

            _context.SaveChanges();
            return Ok(product);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok("Deleted successfully");
        }
    }
}
