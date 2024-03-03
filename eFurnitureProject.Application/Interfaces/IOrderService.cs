using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.OrderViewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetAllOrder();
        Task<ApiResponse<IEnumerable<OrderDetailViewDTO>>> GetOrderDetailById(Guid orderId);
        Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderFilter(int pageIndex, int pageSize, string userId, Guid StatusId);
        Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderPaging(int pageIndex, int pageSize);
        Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO);
    }
}
