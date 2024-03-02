using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _service;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            this._service = orderDetailService;
        }

        //[HttpGet]
        //public async ApiResponse<IEnumerable<OrderDetailViewDTO>> GetOrderDetailById(Guid OrderId)
        //{
        //    return await _service.GetOrderDetailById(OrderId);
        //}
    }
}
