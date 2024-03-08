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

        public async Task<Pagination<ProductDTO>> GetProductsByNameAsync(string productName, int pageIndex, int pageSize)
        {
            var products = await _dbContext.Products
        .Include(p => p.Category)
        .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            InventoryQuantity = p.InventoryQuantity,
            Status = p.Status,
            Price = p.Price,
            CategoryId = p.Category.Id,
            CategoryName = p.Category.Name
        })
        .ToListAsync();

            var totalItems = products.Count;

            var paginatedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,
                 
            };

            return pagination;
        }
        public async Task<Pagination<ProductDTO>> GetProductsByPriceAsync(double minPrice, double maxPrice, int pageIndex, int pageSize)
        {


            IQueryable<Product> query = _dbContext.Products.Include(p => p.Category);

            if (minPrice >= 0 && maxPrice >= 0 && minPrice <=maxPrice)
            {
                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }
            else if (minPrice >= 0)
            {
                query = query.Where(p => p.Price == minPrice);
            }
            else if (maxPrice >= 0)
            {
                query = query.Where(p => p.Price == maxPrice);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            pageIndex = Math.Max(1, Math.Min(pageIndex, totalPages)); // Ensure pageIndex is within valid range

            var paginatedProducts = await query
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    InventoryQuantity = p.InventoryQuantity,
                    Status = p.Status,Price=p.Price,
                    CategoryId = p.Category.Id,
                    CategoryName = p.Category.Name
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,
              
            };

            return pagination;
        }

        





        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIDAsync(Guid categoryId, int pageIndex, int pageSize)
        {
           
            var products = await _dbContext.Products
      .Include(p => p.Category)
      .Where(p => p.CategoryId == categoryId)
      .Select(p => new ProductDTO
      {
          Id = p.Id,
          Name = p.Name,
          Description = p.Description,
          Image = p.Image,
          InventoryQuantity = p.InventoryQuantity,
          Status = p.Status,
          Price = p.Price,
          CategoryId = p.Category.Id,
          CategoryName = p.Category.Name
      })
      .ToListAsync();

            var totalItems = products.Count;

            var paginatedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;
        }



        public async Task<Pagination<ProductDTO>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10)
        {
            
           var query = (from product in _dbContext.Products
                                 join category in _dbContext.Categories
                                 on product.CategoryId equals category.Id
                     
                     select new ProductDTO
                                 {
                                     Id= product.Id,
                                     Name = product.Name,
                                     Description = product.Description,
                                     Image = product.Image,
                                     InventoryQuantity = product.InventoryQuantity,
                                     Status = product.Status,
                         Price = product.Price,
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
        public async Task<Pagination<ProductDTO>> ToPaginationProductNotDeleted(int pageIndex = 0, int pageSize = 10)
        {

            var query = (from product in _dbContext.Products
                         join category in _dbContext.Categories
                         on product.CategoryId equals category.Id
                         where product.IsDeleted == false // san pham chua co xoa
                         select new ProductDTO
                         {
                             Id = product.Id,
                             Name = product.Name,
                             Description = product.Description,
                             Image = product.Image,
                             InventoryQuantity = product.InventoryQuantity,
                             Status = product.Status,
                             Price=product.Price,
                             CategoryId = category.Id,
                             CategoryName = category.Name
                         });

            var totalItemsCount = await query.CountAsync();


            var products = await query
                .OrderByDescending(p => p.InventoryQuantity)
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
