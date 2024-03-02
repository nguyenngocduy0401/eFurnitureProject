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
        Task<IEnumerable<Product>> GetProductPaging(int pageIndex, int pageSize);
        Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
        Task<IEnumerable<Product>> GetProductsByAmountAsync(int amount);
        Task<IEnumerable<Product>> GetProductsByCategoryIDAsync(List<Guid> categoryIds);
        Task<Pagination<Product>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10);
      
    }
}
