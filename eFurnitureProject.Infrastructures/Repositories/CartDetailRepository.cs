using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class CartDetailRepository : ICartDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public CartDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductInCartAsync(CartDetail cartDetail)
        {
            await _dbContext.AddAsync(cartDetail);
        }
    }
}
