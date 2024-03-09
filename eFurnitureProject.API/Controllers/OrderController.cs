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
        public async Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderByFilter ([FromQuery]FilterOrderDTO filterOrderDTO) =>
            await _service.GetOrderFilterAsync(filterOrderDTO);
        
        [HttpGet]
        public async Task<ApiResponse<Pagination<OrderViewForCustomerDTO>>> GetOrderFilterByLogin([FromQuery]FilterOrderByLoginDTO filterOrderByLoginDTO) =>
            await _service.GetOrderFilterByLoginAsync(filterOrderByLoginDTO);
       

        [HttpPut]
        public async Task<ApiResponse<string>> UpdateOrderStatus([FromBody]UpdateOrderStatusDTO updateOrderStatusDTO) =>
            await _service.UpdateOrderStatusAsync(updateOrderStatusDTO);

        [HttpGet]
        public async Task<ApiResponse<OrderDetailViewDTO>> GetOrderById(Guid orderId) =>
            await _service.GetOrderByIdAsync(orderId);

    }
}
