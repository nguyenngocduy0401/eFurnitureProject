using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IOrderDetailService
    {
        Task<ApiResponse<IEnumerable<OrderDetailViewDTO>>> GetOrderDetailById(Guid orderId);
    }
}
