using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewDTO;
using System;
using System.Collections.Generic;
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

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService) { 
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
            _claimsService = claimsService;
        }

        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetAllOrder()
        {
            var response = new ApiResponse<IEnumerable<OrderViewDTO>>();

            try
            {
                var result = await _unitOfWork.OrderRepository.GetAllAsync();
                var viewItems = new List<OrderViewDTO>();

                foreach (var order in result)
                {
                    viewItems.Add(_mapper.Map<OrderViewDTO>(order));
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

        public async Task<ApiResponse<IEnumerable<OrderDetailViewDTO>>> GetOrderDetailById(int pageIndex, int pageSize, Guid orderId)
        {
            var response = new ApiResponse<IEnumerable<OrderDetailViewDTO>>();
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                if (order is null)
                {
                    throw new Exception("Order has not existed!");
                }
                else
                {
                    var viewItems = new List<OrderDetailViewDTO> ();
                    foreach (var item in order.OrderDetail)
                    {
                        viewItems.Add(_mapper.Map<OrderDetailViewDTO>(item));
                    }
                    if(viewItems.Count != 0)
                    {
                        response.Data = viewItems;
                        response.isSuccess = true;
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Data = viewItems;
                        response.isSuccess = true;
                        response.Message = "No record found!";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderFilter(int pageIndex, int pageSize,string UserID, Guid StatusId)
        {
            FilterOrderDTO filterDTO = new FilterOrderDTO();
            filterDTO.UserId = UserID;
            filterDTO.StatusId = StatusId;
            var response = new ApiResponse<IEnumerable<OrderViewDTO>>();

            try
            {
                var result = await _unitOfWork.OrderRepository.GetOrderByFilter(pageIndex, pageSize,filterDTO);
                var viewItems = new List<OrderViewDTO>();

                foreach (var order in result)
                {
                    viewItems.Add(_mapper.Map<OrderViewDTO>(order));
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

        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderPaging(int pageIndex, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<OrderViewDTO>>();

            try
            {
                var result = await _unitOfWork.OrderRepository.Get(pageIndex, pageSize);
                var viewItems = new List<OrderViewDTO>();

                foreach (var order in result)
                {
                    viewItems.Add(_mapper.Map<OrderViewDTO>(order));
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

        public Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            // can phai hoi lai nghiep vu

            throw new NotImplementedException();
        }


    }
}
