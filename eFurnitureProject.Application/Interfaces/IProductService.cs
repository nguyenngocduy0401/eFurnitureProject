using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ProductViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<ProductViewDTO>>> GetProductsInPageAsync(int page, int amount);
        Task<ApiResponse<IEnumerable<ProductViewDTO>>> GetFilterProductsInPageAsync(int page, int amount, string searchValue);
        Task<ApiResponse<ProductViewDTO>> GetProductDetail(Guid productId);
    }
}
