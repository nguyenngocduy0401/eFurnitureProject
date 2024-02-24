using eFurnitureProject.Application.Commons;
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
        Task<ApiResponse<IEnumerable<ProductDTO>>> getAllProduct();
        Task<ApiResponse<ProductDTO>> GetProductByID(int id);
        Task<ApiRespons<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO,  Guid productID);
        Task<ApiRespons<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID);
    }
}
