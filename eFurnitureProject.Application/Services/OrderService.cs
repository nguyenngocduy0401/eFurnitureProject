using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService) 
        { 
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
            _claimsService = claimsService;
        }

        public async Task<ApiResponse<string>> CheckOutOrder(CreateOrderDTO createOrderDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                

            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    

        public async Task<ApiResponse<OrderDetailViewDTO>> GetOrderByIdAsync(Guid orderId)
        {
            var response = new ApiResponse<OrderDetailViewDTO>();
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderFilterAsync(FilterOrderDTO filterOrderDTO)
        {
            var response = new ApiResponse<Pagination<OrderViewDTO>>();
            try 
            {
                var listOrder = await _unitOfWork.OrderRepository.GetOrderByFilter
                    (filterOrderDTO.PageIndex, filterOrderDTO.PageSize,
                     filterOrderDTO.StatusCode, filterOrderDTO.FromTime,
                     filterOrderDTO.ToTime, filterOrderDTO.Search);
                if (listOrder == null) 
                {   
                    response.isSuccess = true;
                    response.Message = "Not found!";
                    return response;
                }
                var result = _mapper.Map<Pagination<OrderViewDTO>>(listOrder);
                response.Data = result; 
                response.isSuccess = true;
                response.Message = "Successful!";
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<Pagination<OrderViewForCustomerDTO>>> GetOrderFilterByLoginAsync(FilterOrderByLoginDTO filterOrderByLogin)
        {
            var response = new ApiResponse<Pagination<OrderViewForCustomerDTO>>();
            try
            {
                var userId = _claimsService.GetCurrentUserId.ToString();
                if (userId == null) 
                {
                    response.isSuccess = false;
                    response.Message = "login first!";
                }
                var listOrder = await _unitOfWork.OrderRepository.GetOrderFilterByLogin
                    (filterOrderByLogin.PageIndex, filterOrderByLogin.PageSize, 
                     filterOrderByLogin.StatusCode, filterOrderByLogin.FromTime, 
                     filterOrderByLogin.ToTime, userId);
                if (listOrder == null)
                {
                    response.isSuccess = true;
                    response.Message = "Not found!";
                    return response;
                }
                var result = _mapper.Map<Pagination<OrderViewForCustomerDTO>>(listOrder);
                response.Data = result;
                response.isSuccess = true;
                response.Message = "Successful!";
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<StatusDetailOrderViewDTO>> GetOrderStatusByOrderId(Guid orderId)
        {
            var response = new ApiResponse<StatusDetailOrderViewDTO>();
            try
            {
                var statusDetail = await _unitOfWork.OrderRepository.GetStatusOrderByOrderId(orderId);
                if (statusDetail == null)
                {
                    response.isSuccess = false;
                    response.Message = "Not found!";
                }
                var result = _mapper.Map<StatusDetailOrderViewDTO>(statusDetail);
                response.Data = result;
                response.isSuccess = true;
                response.Message = "Get status successfully!";
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                var newStatus = await _unitOfWork.StatusOrderRepository.GetGuidByStatusCode(updateOrderStatusDTO.StatusCode);
                var newOrder = await _unitOfWork.OrderRepository.GetByIdAsync(updateOrderStatusDTO.Id);
                if (newOrder == null) 
                {
                    response.isSuccess = false;
                    response.Message = "Not found order!";
                    return response;
                }

                var oldStatus = await _unitOfWork.StatusOrderRepository.GetByIdAsync((Guid)newOrder.StatusId);
                if (updateOrderStatusDTO.StatusCode <= oldStatus.StatusCode) 
                {
                    response.isSuccess = false;
                    response.Message = "Invalid state!";
                    return response;
                }
                newOrder.StatusId = newStatus.Id;
                _unitOfWork.OrderRepository.Update(newOrder);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (!isSuccess)
                {
                    response.isSuccess = false;
                    response.Message = "Update fail!";
                }
                response.isSuccess = true;
                response.Message = "Successful!";
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }   
    }
}
