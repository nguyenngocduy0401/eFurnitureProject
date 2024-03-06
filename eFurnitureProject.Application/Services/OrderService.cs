using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewDTO;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    var viewItems = new Collection<OrderDetailViewDTO>();
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



                            if (item.Product is not null)

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

        public async Task<ApiResponse<IEnumerable<OrderViewDTO>>> GetOrderFilter(int pageIndex, int pageSize, string UserID, Guid StatusId)
        {
            FilterOrderDTO filterDTO = new FilterOrderDTO();
            filterDTO.UserId = UserID;
            filterDTO.StatusId = StatusId;
            var response = new ApiResponse<IEnumerable<OrderViewDTO>>();

            try
            {
                var result = await _unitOfWork.OrderRepository.GetOrderByFilter(pageIndex, pageSize, filterDTO);
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
                    var viewItem = _mapper.Map<OrderViewDTO>(order);
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

        public async Task<ApiResponse<UpdateOrderStatusDTO>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            // can phai hoi lai nghiep vu

            var response = new ApiResponse<UpdateOrderStatusDTO>();

            try
            {
                //var result = await _unitOfWork.OrderRepository.Get(pageIndex, pageSize);
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(updateOrderStatusDTO.Id);

                if (order is not null)
                {
#pragma warning disable CS8629 // Nullable value type may be null.
                    Guid statusID = (Guid)order.StatusId;
#pragma warning restore CS8629 // Nullable value type may be null.
                    if (order.StatusOrder != null)
                    {
                        var orderStatus = await _unitOfWork.StatusOrderRepository.GetByIdAsync(statusID);
                        if (orderStatus is not null)
                        {
                            orderStatus.StatusCode = updateOrderStatusDTO.StatusCode;
                            orderStatus.Name = CreateStatusOrder(updateOrderStatusDTO.StatusCode);
                        }
                        else
                        {
                            throw new Exception("Update Status order fail!");
                        }
                    }
                    else
                    {
                        throw new Exception("Order status not found!");
                    }
                    int update = await _unitOfWork.SaveChangeAsync();
                    if (update > 0)
                    {
                        response.Data = updateOrderStatusDTO;
                        response.isSuccess = true;
                        response.Message = "Update Succesfully";
                    }
                    else
                    {
                        throw new Exception("Update failled");
                    }
                }
                else
                {
                    throw new Exception("Order not found!");
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

        public async Task<ApiResponse<OrderViewDTO>> CreateNewOrder(CreateOrderDTO Draft)
        {
            var response = new ApiResponse<OrderViewDTO>();
            var newStatusOrder = new StatusOrder();
            newStatusOrder.StatusCode = 1;
            newStatusOrder.Name = StatusOrderEnum.Awaiting.ToString();
            var newOrder = new Order();
            await _unitOfWork.OrderRepository.AddAsync(newOrder);

            try
            {
                var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(Draft.TransactionId);
                List<OrderDetail> Products = Draft.orderDetails.ToList();
                if (transaction is null)
                {
                    throw new Exception("Create fail by not found transaction");
                }
                
                foreach (var product in Products)
                {
                    var item = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
                    if (item is null)
                    {
                        throw new Exception("Product not found!");
                    }
                    else
                    {
                        var newOrderDetail = new OrderDetail();
                        newOrderDetail.ProductId = item.Id;
                        newOrderDetail.OrderId = newOrder.Id;
                        newOrderDetail.Quantity = product.Quantity;
                        newOrderDetail.Price = product.Price;
                        await _unitOfWork.OrderDetailRepository.AddAsync(newOrderDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Data= null;
                response.isSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }


        private string CreateStatusOrder(int code)
        {
            switch (code)
            { 
                case 1: return StatusOrderEnum.Awaiting.ToString();
                case 2: return StatusOrderEnum.Delivering.ToString();
                case 3: return StatusOrderEnum.Cancel.ToString();
                case 4: return StatusOrderEnum.Success.ToString();
                case 5: return StatusOrderEnum.Refuse.ToString();
                default: return "";
            }
        }
    }

}
