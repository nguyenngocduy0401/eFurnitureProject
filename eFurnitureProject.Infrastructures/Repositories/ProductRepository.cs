using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ProductDTO;
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

        public async Task<IEnumerable<Product>> GetAll2(int page, string CategoryName, string ProductName, int amount, int pageSize)
        {
            return await _dbContext.Products.ToListAsync();
        }
     

        public  Task<IEnumerable<Product>> GetAll(int page, string CategoryName, string ProductName, int amount, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductViewDTO>> GetAll2(int page, List<Guid> categoryId, string ProductName, int amount, int pageSize)
        {
            IQueryable<ProductViewDTO> query = _dbContext.Products;

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
    }
}
