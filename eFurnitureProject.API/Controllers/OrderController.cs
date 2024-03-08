using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
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
        [HttpGet]
        public async Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderByFilter ([FromQuery]FilterOrderDTO filterOrderDTO)
        {
            return await _service.GetOrderFilterAsync(filterOrderDTO);
        }
        
        [HttpPost]
        public async Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatus([FromBody]UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            return await _service.UpdateOrderStatusAsync(updateOrderStatusDTO);
        }
        [HttpGet]
        public async Task<ApiResponse<OrderDetailViewDTO>> GetOrderById(Guid orderId)
        {
            return await _service.GetOrderByIdAsync(orderId);
        }
    }
}
