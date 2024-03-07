using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
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

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService) { 
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
                    var viewItems = new Collection<OrderDetailViewDTO> ();
                    if (order.OrderDetail is null)
                    {
                        response.Data = viewItems;
                        response.isSuccess = true;
                        response.Message = "No product in order";
                    }
                    else
                    {
                        foreach (var item in order.OrderDetail)
                        {
                           var viewItem = _mapper.Map<OrderDetailViewDTO>(item);



                            viewItem.Product = _mapper.Map<ProductViewDTO>(item.Product);



                            if(item.Product is not null)

                            {

                                if (item.Product.Category is not null)

                                {

                                    viewItem.Product.CategoryName = item.Product.Category.Name;

                                }

                            }



                            viewItems.Add(viewItem);
                        }
                        if (viewItems.Count != 0)
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

        

        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderFilterAsync(ViewModels.OrderViewModels.FilterOrderDTO filterOrderDTO)
        {
            var response = new ApiResponse<IEnumerable<OrderViewDTO>>();
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
        

        public async Task<ApiResponse<IEnumerable<OrderViewGetDTO>>> GetOrderPaging(int pageIndex, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<OrderViewGetDTO>>();

            try
            {
                var result = await _unitOfWork.OrderRepository.Get(pageIndex, pageSize);
                var viewItems = new List<OrderViewGetDTO>();

                foreach (var order in result)
                {
                    var viewItem = _mapper.Map<OrderViewGetDTO>(order);
                    if (order.User != null)
                    {
                        viewItem.Name = order.User.Name;
                    }
                    else
                    {
                        viewItem.Name = "Guest";
                    }

                    if (order.StatusOrder != null)
                    {
                        viewItem.StatusCode = order.StatusOrder.StatusCode;
                    }
                    else
                    {
                        viewItem.StatusCode = 0;
                    }


                    if (order.Transaction != null)
                    {
                        viewItem.Paid = order.Transaction.Amount;
                    }
                    else
                    {
                        viewItem.Paid = 0;
                    }

                    viewItems.Add(viewItem);
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
