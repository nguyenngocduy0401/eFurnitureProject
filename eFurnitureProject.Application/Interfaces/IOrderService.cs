﻿using eFurnitureProject.Application.Commons;
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
        Task<ApiResponse<OrderDetailViewDTO>> GetOrderByIdAsync(Guid orderId);
        Task<ApiResponse<Pagination<OrderViewForCustomerDTO>>> GetOrderFilterByLoginAsync(FilterOrderByLoginDTO filterOrderByLogin);
        Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderFilterAsync(FilterOrderDTO filterOrderDTO);
        Task<ApiResponse<string>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO);
    }
}
