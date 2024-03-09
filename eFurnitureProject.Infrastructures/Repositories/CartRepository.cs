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
    public  class CartRepository :GenericRepository<Cart> , ICartRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IClaimsService _claimsService;

        public CartRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _claimsService = claimsService;
        }

        public async Task<Cart> GetCartAsync()
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.UserId == _claimsService.GetCurrentUserId.ToString());
            if (result == null)
            {
                throw new Exception($"Not found any cart with userId: {_claimsService.GetCurrentUserId}");
            }
            return result;
            }
    }
    
 }
