using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.OrderViewDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        public OrderRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        public async Task<IEnumerable<Order>> Get(int pageIndex, int pageSize)
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

        public async Task<IEnumerable<Order>> GetOrderByFilter(int pageIndex, int pageSize, FilterOrderDTO filter)
        {
            //Expression<Func<Order, bool>> order = new Expression<Func<Order, bool>>;

            try
            {
                var items = await _dbSet
                    .Where(o => o.UserId == filter.UserId)
                    .Where(o => o.StatusId == filter.StatusId)
                    .OrderByDescending(x => x.CreationDate)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
                return items;
            }
            catch (Exception)
            {

                throw new Exception();
            }
            //return null;
        }


        public async Task<IEnumerable<Order>> GetOrderByStatus(int pageIndex, int pageSize, Guid statusId)
        {
            //Expression<Func<Order, bool>> order = new Expression<Func<Order, bool>>;

            try
            {
                var items = await _dbSet
                    .Where(o => o.StatusId == statusId)
                    .OrderByDescending(x => x.CreationDate)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
                return items;
            }
            catch (Exception)
            {

                throw new Exception();
            }
            //return null;
        }
    }
}
