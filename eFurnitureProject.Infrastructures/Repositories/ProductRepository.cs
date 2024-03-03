using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public ProductRepository(
            AppDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<IEnumerable<Product>> GetProductPaging(int pageIndex, int pageSize)
        {
            try
            {
                var items = await _dbSet.OrderByDescending(x => x.CreationDate)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
                return items;
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName)
        {
            var products = await _dbContext.Products
                 .Include(p => p.Category) 
                 .Where(p => p.Category.Name == categoryName) 
                 .ToListAsync(); 

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string productName)
        {
            var products = await _dbContext.Products.Where(u => u.Name == productName).ToListAsync();
               

            return products;
        }

        public async void IncreaseQuantityProductFromImport(ICollection<ImportDetail> importDetails)
        {
            List<Product> products = new List<Product>();
            foreach (var importDetail in importDetails)
            {
                var product = await _dbContext.Products.FirstAsync(x => x.Id == importDetail.ProductId);
                product.InventoryQuantity += importDetail.Quantity;
                product.ModificationDate = _timeService.GetCurrentTime();
                product.ModificationBy = _claimsService.GetCurrentUserId;
                products.Add(product);
            }
            _dbSet.UpdateRange(products);
        }
    }
}
