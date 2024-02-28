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
        Task<IEnumerable<Product>> GetAll(int page, string CategoryName, string ProductName, int amount,  int pageSize);
        Task<IEnumerable<Product>> GetProductsByAmountAsync(int amount);
        Task<IEnumerable<Product>> GetAll2(int page, List<Guid> categoryId, string ProductName, int amount, int pageSize);
    }
}
