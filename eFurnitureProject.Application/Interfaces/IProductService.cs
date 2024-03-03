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
        Task<ApiResponse<IEnumerable<ProductDTO>>> GetAll(int page, List<Guid> CategoryID, string ProductName, int amount, int pageSize);
        Task<ApiResponse<Pagination<ProductDTO>>> getAllProduct(int pageIndex = 0, int pageSize = 10);
        Task<ApiResponse<ProductDTO>> GetProductByID(Guid id);
        Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO);
        Task<ApiResponse<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID);
        Task<ApiResponse<bool>> DeleteProduct(Guid productID);
        Task<ApiResponse<Pagination<ProductDTO>>> getAllProductNotdeleted(int pageIndex = 0, int pageSize = 10);


    }
}
