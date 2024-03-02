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
        protected DbSet<OrderDetail> _dbSet;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public OrderDetailRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService)
        {
            _dbSet = context.Set<OrderDetail>();
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
