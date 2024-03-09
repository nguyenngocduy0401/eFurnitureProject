using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class FeedbackRepository :GenericRepository<Feedback> ,IFeedbackRepository
    {
        private readonly AppDbContext _dbContext;
        public FeedbackRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
          
        }

        public async Task<bool> CheckProduct(Guid productId)
        {
            var isProductInStatus4 = await _dbContext.Products
               .Join(_dbContext.OrdersDetails, p => p.Id, od => od.ProductId, (p, od) => new { Product = p, OrderDetail = od })
               .Join(_dbContext.Orders, od => od.OrderDetail.OrderId, o => o.Id, (od, o) => new { od.Product, od.OrderDetail, Order = o })
               .Join(_dbContext.StatusOrders, o => o.Order.StatusId, so => so.Id, (o, so) => new { o.Product, o.OrderDetail, o.Order, StatusOrder = so })
               .Where(joinResult => joinResult.StatusOrder.StatusCode == 4 && joinResult.Product.Id == productId)
               .AnyAsync();

            return isProductInStatus4;
        }
    }
}
