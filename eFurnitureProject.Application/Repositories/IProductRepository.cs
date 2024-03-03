using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
      
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryNameAsync(string categoryName);
        Task<IEnumerable<ProductDTO>> GetProductsByNameAsync(string productName);
        Task<IEnumerable<ProductDTO>> GetProductsByAmountAsync(int amount);
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryIDAsync(List<Guid> categoryIds);
        Task<Pagination<ProductDTO>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10);
       


    }
}
