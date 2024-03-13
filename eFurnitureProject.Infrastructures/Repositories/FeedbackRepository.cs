﻿using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
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
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        public FeedbackRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _currentTime = timeService;
            _claimsService = claimsService;
        }


        public async Task<bool> CheckProduct(Guid productId)
        {
            var userIdcurrent = _claimsService.GetCurrentUserId.ToString();
           
            // Kiểm tra xem sản phẩm có trong trạng thái 4 không và người đăng nhập có phải là người đặt hàng không
            var isProductInStatus4 = await _dbContext.Products
               .Join(_dbContext.OrdersDetails, p => p.Id, od => od.ProductId, (p, od) => new { Product = p, OrderDetail = od })
               .Join(_dbContext.Orders, od => od.OrderDetail.OrderId, o => o.Id, (od, o) => new { od.Product, od.OrderDetail, Order = o })
               .Join(_dbContext.StatusOrders, o => o.Order.StatusId, so => so.Id, (o, so) => new { o.Product, o.OrderDetail, o.Order, StatusOrder = so })
               .Where(joinResult => joinResult.StatusOrder.StatusCode == 4 &&
                                     joinResult.Product.Id == productId &&
                                     joinResult.Order.UserId == userIdcurrent) // Kiểm tra userIdcurrent có phải là người đặt hàng
               .AnyAsync();

            return isProductInStatus4;
        }
      public async   Task<Pagination<FeedBackViewDTO>> GetFeedBacksByUserID(int pageIndex, int pageSize, string userID)
        {
            var feedbackList = await _dbContext.Feedbacks
        .Where(f => f.UserId == userID)
        .OrderByDescending(f => f.CreationDate)
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync();

            var totalCount = await _dbContext.Feedbacks
                .Where(f => f.UserId == userID)
                .CountAsync();

            var feedbackViewList = feedbackList.Select(f => new FeedBackViewDTO
            {
                UserId = f.UserId,
                Details = f.Details,
                Title = f.Title,
                ProductId = f.ProductId,
                ProductName = f.Product.Name
            }).ToList();

            var pagination = new Pagination<FeedBackViewDTO>
            {
                Items = feedbackViewList,
                TotalItemsCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return pagination;
        }
    }
}
