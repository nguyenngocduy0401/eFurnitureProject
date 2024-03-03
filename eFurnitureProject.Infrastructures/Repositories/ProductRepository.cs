using eFurnitureProject.Application.Commons;
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

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? InventoryQuantity { get; set; }
        public int Status { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryNameAsync(string categoryName)
        {
            var product = await _dbContext.Products
                            .Include(p => p.Category)
                            .Where(p => p.Category.Name == categoryName)
                            .Select(p => new ProductDTO
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Image = p.Image,
                                InventoryQuantity = p.InventoryQuantity,
                                Status = p.Status,
                                CategoryId=p.Category.Id,
                                CategoryName = p.Category.Name
                            })
                            .ToListAsync();

            return product;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByNameAsync(string productName)
        {
            var product = await _dbContext.Products
                            .Include(p => p.Category)
                            .Where(p => p.Name == productName)
                            .Select(p => new ProductDTO
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Image = p.Image,
                                InventoryQuantity = p.InventoryQuantity,
                                Status = p.Status,
                                CategoryId = p.Category.Id,
                                CategoryName = p.Category.Name
                            })
                            .ToListAsync();

            return product;
        }
        public async Task<IEnumerable<ProductDTO>> GetProductsByAmountAsync(int amount)
        {
            var product = await _dbContext.Products
                            .Include(p => p.Category)
                            .Where(p => p.InventoryQuantity == amount)
                            .Select(p => new ProductDTO
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Image = p.Image,
                                InventoryQuantity = p.InventoryQuantity,
                                Status = p.Status,
                                CategoryId = p.Category.Id,
                                CategoryName = p.Category.Name
                            })
                            .ToListAsync();

            return product;
        }

       
     

      
        public async Task<IEnumerable<ProductDTO>> GetAll(int page, List<Guid> categoryId, string ProductName, int amount, int pageSize )
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
            var result = await query.Join(
           _dbContext.Categories,
           product => product.CategoryId,
           category => category.Id,
           (product, category) => new ProductDTO
           {
               Id = product.Id,
               Name = product.Name,
               CategoryId = product.CategoryId,
               CategoryName = category.Name,
               InventoryQuantity = product.InventoryQuantity
           })
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
            return result;

        }
        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIDAsync(List<Guid> categoryIds)
        {
            var product = await _dbContext.Products
         .Include(p => p.Category)
         .Where(p => categoryIds.Contains(p.CategoryId.GetValueOrDefault())) // Close the Contains method call here
         .Select(p => new ProductDTO
         {
             Id = p.Id,
             Name = p.Name,
             Description = p.Description,
             Image = p.Image,
             InventoryQuantity = p.InventoryQuantity,
             Status = p.Status,
             CategoryId = p.Category.Id,
             CategoryName = p.Category.Name
         })
         .ToListAsync();

            return product;
        }



      

      

       



        public async Task<Pagination<ProductDTO>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10)
        {
            
           var query = (from product in _dbContext.Products
                                 join category in _dbContext.Categories
                                 on product.CategoryId equals category.Id
                     where product.IsDeleted == false // san pham chua co xoa
                     select new ProductDTO
                                 {
                                     Id= product.Id,
                                     Name = product.Name,
                                     Description = product.Description,
                                     Image = product.Image,
                                     InventoryQuantity = product.InventoryQuantity,
                                     Status = product.Status,
                                     CategoryId = category.Id,
                                     CategoryName = category.Name
                                 });

                    var totalItemsCount = await query.CountAsync();

                   
                    var products = await query
                        .OrderByDescending(p => p. InventoryQuantity) 
                        .Skip(pageIndex * pageSize) 
                        .Take(pageSize) 
                        .ToListAsync();

                    
                    var pagination = new Pagination<ProductDTO>
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        TotalItemsCount = totalItemsCount,
                        Items = products
                    };

                    return pagination;
                }

      
    }
}
