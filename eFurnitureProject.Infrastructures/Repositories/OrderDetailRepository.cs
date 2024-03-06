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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public OrderDetailRepository(AppDbContext dbContext, ICurrentTime current, IClaimsService claimsService)
        {
            _dbContext = dbContext;
            _timeService = current;
            _claimsService = claimsService;
        }
        public async Task AddAsync(OrderDetail newOrderDetail)
        {
            await _dbContext.AddAsync(newOrderDetail);
        }
    }
}
