using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<IEnumerable<OrderViewGetDTO>>> GetAllOrder();
        Task<ApiResponse<IEnumerable<OrderDetailViewDTO>>> GetOrderDetailById(int pageIndex, int pageSize, Guid orderId);
        Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderFilterAsync(FilterOrderDTO filterOrderDTO);
        Task<ApiResponse<IEnumerable<OrderViewGetDTO>>> GetOrderPaging(int pageIndex, int pageSize);
        Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO);
    }
}
