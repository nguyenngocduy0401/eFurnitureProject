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
    public class ProductRepository:GenericRepository<Product>,IProductRepository
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
    }
}
