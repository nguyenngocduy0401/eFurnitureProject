using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<ProductDTO>>> GetAll(int page, string CategoryName, string ProductName, int amount, int pageSize);
        Task<ApiResponse<IEnumerable<ProductViewDTO>>> GetProductsInPageAsync(int page, int amount);
        Task<ApiResponse<IEnumerable<ProductViewDTO>>> GetFilterProductsInPageAsync(int page, int amount, string searchValue);
        Task<ApiResponse<ProductViewDTO>> GetProductDetail(Guid productId);
        Task<ApiResponse<IEnumerable<ProductDTO>>> getAllProduct();
        Task<ApiResponse<ProductDTO>> GetProductByID(Guid id);
        Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO);
        Task<ApiResponse<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID);
        Task<ApiResponse<bool>> DeleteProduct(Guid productID);
        Task<ApiResponse<IEnumerable<ProductDTO>>> SearchProductByNameAsync(string name);
        Task<ApiResponse<IEnumerable<ProductDTO>>> SearchProductByCategoryNameAsync(string name);
        Task<ApiResponse<IEnumerable<ProductViewDTO>>> FilterProducts(int page, List<Guid> categoryId, string productName, int amount, int pageSize);
    }
}
