﻿using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatus([FromBody]UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            return await _service.UpdateOrderStatusAsync(updateOrderStatusDTO);
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetAllOrder()
        {
            return await _service.GetAllOrder();
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderPaging(int pageIndex, int pageSize)
        {
            return await _service.GetOrderPaging(pageIndex, pageSize);
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> FilterOrder(string UserID, Guid StatusId, int pageIndex, int pageSize)
        {
            return await _service.GetOrderFilter(pageIndex, pageSize, UserID, StatusId);
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OrderDetailViewDTO>>> GetOrderDetailById(int pageIndex,int pageSize, Guid OrderId)
        {
            return await _service.GetOrderDetailById(pageIndex, pageSize, OrderId);
        }

        //[HttpGet]
        //public async Task<ApiResponse<int>> GetTotalAmountOrder()
        //{
        //    return await _service.GetTotalAmountOrder();
        //}
    }
}
