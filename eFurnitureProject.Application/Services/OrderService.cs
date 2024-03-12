using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.CartViewModels;
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
        public async Task<ApiResponse<string>> CheckOut(CreateOrderDTO createOrderDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                var userId = _claimsService.GetCurrentUserId.ToString();
                var cartDetails = await _unitOfWork.CartRepository.GetCartDetailsByUserId(userId);
                var createOrder = _mapper.Map<Order>(createOrderDTO);
                await _unitOfWork.OrderRepository.AddAsync(createOrder);
                var resultCreate = await _unitOfWork.SaveChangeAsync() > 0;
                if (!resultCreate) throw new Exception("Order creation failed!");
                var id = createOrder.Id;
                var price = 0d;
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var cartDetail in cartDetails)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartDetail.ProductId);
                    if (product == null) throw new Exception($"Some products do not exist in your shopping cart!");
                    if (product.IsDeleted) throw new Exception($"{product.Name} do not exist in your shopping cart!");
                    if (product.Status != 2) throw new Exception($"{product.Name} has been discontinued!");
                    if (product.InventoryQuantity <=0) throw new Exception($"{product.Name} out of stock!");
                        orderDetails.Add(new OrderDetail
                    {
                        ProductId = id,
                        Quantity = cartDetail.Quantity,
                        Price = product.Price,
                    });
                    price =+ product.Price * cartDetail.Quantity;
                    
                }

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
