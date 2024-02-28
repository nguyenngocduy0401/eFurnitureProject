using eFurnitureProject.Application.Commons;
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

        public ProductRepository(
            AppDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
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
        public async Task<IEnumerable<Product>> GetProductsByAmountAsync(int amount)
        {
            return await _dbContext.Products
                .Where(p => p.InventoryQuantity >= amount)
                .ToListAsync();
        }

       
     

      
        public async Task<IEnumerable<Product>> GetAll(int page, List<Guid> categoryId, string ProductName, int amount, int pageSize)
        {
            IQueryable<Product> query = _dbContext.Products;

            // Apply filters
            if (categoryId != null && categoryId.Any())
            {
                query = query.Where(p => categoryId.Contains(p.CategoryId.Value));
            }
            if (!string.IsNullOrEmpty(ProductName))
            {
                query = query.Where(p => p.Name.Contains(ProductName));
            }
            if (amount > 0)
            {
                query = query.Where(p => p.InventoryQuantity >= amount);
            }

            // Paginate the results
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryIDAsync(List<Guid> categoryIds)
        {
            IQueryable<Product> query = _dbContext.Products;
            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(p => categoryIds.Contains(p.CategoryId.Value));
            }

            return await query.ToListAsync();
        }



      

      

        public async Task<Pagination<Product>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10)
        {
            var query = _dbSet.AsQueryable();

            // Thêm điều kiện isDelete == false vào truy vấn
            query = query.Where(x => x.IsDeleted == false);

            var itemCount = await query.CountAsync();
            var items = await query.OrderByDescending(x => x.CreationDate)
                                   .Skip(pageIndex * pageSize)
                                   .Take(pageSize)
                                   .AsNoTracking()
                                   .ToListAsync();

            var result = new Pagination<Product>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
    }
}
