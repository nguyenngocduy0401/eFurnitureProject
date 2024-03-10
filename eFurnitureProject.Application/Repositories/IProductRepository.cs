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
        Task<Pagination<ProductDTO>> GetProductsByNameAsync(string productName, int pageIndex, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByPriceAsync(double minPrice, double maxPrice, int pageIndex, int pageSize);

        Task<Pagination<ProductDTO>> ToPaginationProductNotDeleted(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIDAsync(Guid categoryIds, int pageIndex, int pageSize);
        Task<Pagination<ProductDTO>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10);


        void IncreaseQuantityProductFromImport(ICollection<ImportDetail> importDetails);

        Task<int> GetQuantityByIdAsync(Guid productId);
    }
}
