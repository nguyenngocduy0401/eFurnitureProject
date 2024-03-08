using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
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

        public async Task<ApiResponse<IEnumerable<OrderViewGetDTO>>> GetAllOrder()
        {
            var response = new ApiResponse<IEnumerable<OrderViewGetDTO>>();

            try
            {
                var result = await _unitOfWork.OrderRepository.GetAllAsync();
                var viewItems = new List<OrderViewGetDTO>();

                foreach (var order in result)
                {
                    viewItems.Add(_mapper.Map<OrderViewGetDTO>(order));
                }

                if (viewItems.Count != 0)
                {
                    response.Data = viewItems;
                    response.isSuccess = true;
                    response.Message = "Success!";
                }
                else
                {
                    response.Data = null;
                    response.isSuccess = true;
                    response.Message = "No reocrd!";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
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
                return response;
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

        public Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            // can phai hoi lai nghiep vu

            throw new NotImplementedException();
        }


    }
}
