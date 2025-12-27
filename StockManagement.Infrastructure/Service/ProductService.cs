using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Application;
using StockManagement.Core.Application.DTOs;
using StockManagement.Core.Application.InterFases;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;

namespace StockManagement.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly StockDbConnect _context;

        public ProductService(StockDbConnect context)
        {
            _context = context;
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                ProductName = createProductDto.Name,
                Quantity = createProductDto.Quantity,
                Price = createProductDto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<ProductResponseDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            product.ProductName = updateProductDto.Name;
            product.Quantity = updateProductDto.Quantity;
            product.Price = updateProductDto.Price;

            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.id == id);
        }

        private ProductResponseDto MapToDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.id,
                Name = product.ProductName,
                Quantity = product.Quantity,
                Price = product.Price,
                CreatedAt = product.CreatedAt
            };
        }
    }
}