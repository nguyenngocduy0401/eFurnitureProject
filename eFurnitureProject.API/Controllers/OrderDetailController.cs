using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            this.orderDetailService = orderDetailService;
        }

        [HttpGet]
        public ApiResponse<IEnumerable<OrderDetailViewDTO>> GetOrderDetailById(Guid OrderId)
        {
            return null;
        }

        //public async ApiResponse<IEnumerable<OrderDetailViewDTO>> GetTop5Product()
        //{
        //    return await orderDetailService.GetTop5Product();
        //}
    }
}
