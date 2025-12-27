using StockManagement.Core.Application.DTOs;

namespace StockManagement.Core.Application.InterFases
{
    public interface IProductService
    {
        Task<ProductResponseDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductResponseDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
}