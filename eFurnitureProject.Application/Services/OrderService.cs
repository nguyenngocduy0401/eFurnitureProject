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
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly UserManager<User> _userManager;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService,
                            UserManager<User> userManager) 
        { 
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
            _claimsService = claimsService;
            _userManager = userManager;
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
                var newStatus = await _unitOfWork.StatusOrderRepository.GetStatusByStatusCode(updateOrderStatusDTO.StatusCode);
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
                bool checkVoucher = false;
                var voucherInfo = new Voucher();
                var userId = _claimsService.GetCurrentUserId.ToString();
                if (!createOrderDTO.voucherId.Equals(null))
                {
                    //Check voucher existed
                    voucherInfo = await _unitOfWork.VoucherRepository.GetByIdAsync((Guid)createOrderDTO.voucherId);
                    if (voucherInfo == null || voucherInfo.IsDeleted || voucherInfo.Number <= 0) throw new Exception("Not found voucher!");
                    //Check voucher be used
                    if (await _unitOfWork.VoucherDetailRepository.CheckVoucherBeUsedByUser(userId, (Guid)createOrderDTO.voucherId)) 
                        throw new Exception("Voucher is used!");
                    else 
                        checkVoucher = true;
                }
                var cartDetails = await _unitOfWork.CartRepository.GetCartDetailsByUserId(userId);
                var createOrder = _mapper.Map<Order>(createOrderDTO);
                await _unitOfWork.OrderRepository.AddAsync(createOrder);
                var resultCreate = await _unitOfWork.SaveChangeAsync() > 0;
                if (!resultCreate) throw new Exception("Order creation failed!");
                var id = createOrder.Id;
                var price = 0d;
                // insert product from cart to orderDetail
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                List<Product> products = new List<Product>();
                foreach (var cartDetail in cartDetails)
                {
                    
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartDetail.ProductId);
                    if (product == null) throw new Exception($"Some products do not exist in your shopping cart!");
                    if (product.IsDeleted) throw new Exception($"{product.Name} do not exist in your shopping cart!");
                    if (product.Status != 2) throw new Exception($"{product.Name} has been discontinued!");
                    if (product.InventoryQuantity <=0) throw new Exception($"{product.Name} out of stock!");
                    product.InventoryQuantity = product.InventoryQuantity - cartDetail.Quantity;
                    products.Add(product);
                    orderDetails.Add(new OrderDetail
                    {
                        OrderId = id,
                        ProductId = product.Id,
                        Quantity = cartDetail.Quantity,
                        Price = product.Price,
                    });
                    price =+ product.Price * cartDetail.Quantity;  
                }
                await _unitOfWork.OrderDetailRepository.AddRangeAsync(orderDetails);
                _unitOfWork.ProductRepository.UpdateProductByOrder(products);
                if (checkVoucher)
                {
                    if (voucherInfo.MinimumOrderValue <= price)
                    {
                        var discount = voucherInfo.Percent * price;
                        if (discount > voucherInfo.MaximumDiscountAmount)
                        {
                            price = price - voucherInfo.MaximumDiscountAmount;
                        }
                        else price = price - discount;
                    }
                }
                createOrder.Price = price;
                createOrder.Address = createOrderDTO.Address;
                createOrder.Email = createOrderDTO.Email;
                createOrder.PhoneNumber = createOrderDTO.PhoneNumber;
                createOrder.StatusId = (await _unitOfWork.StatusOrderRepository.GetStatusByStatusCode(1)).Id;
                createOrder.Name = createOrderDTO.Name;

                var user = await _userManager.FindByIdAsync(userId);
                if (user.Wallet < price) throw new Exception("Not enough money!");
                user.Wallet = user.Wallet - price;
                await _userManager.UpdateAsync(user);
                _unitOfWork.OrderRepository.Update(createOrder);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (!isSuccess) throw new Exception("Create fail!");
                response.isSuccess = true;
                response.Message = "Checkout Successfully!";

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
