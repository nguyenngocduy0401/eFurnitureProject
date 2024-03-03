using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FluentValidation;
using FluentValidation.Results;

namespace eFurnitureProject.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMemoryCache _memoryCache;
        private readonly IValidator<CreateProductDTO> _createProductvalidator;
       

        public ProductService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMemoryCache memoryCache, IProductRepository productRepository,IValidator<CreateProductDTO> validatorCreateProduct)
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _memoryCache = memoryCache;
            _createProductvalidator = validatorCreateProduct;
         
        }
        public async Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var product = _mapper.Map<Product>(createProductDTO);
                ValidationResult validationResult = await _createProductvalidator.ValidateAsync(createProductDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    await _unitOfWork.ProductRepository.AddAsync(product);
                    var addSuccessfully = await _unitOfWork.SaveChangeAsync();
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    response.Data = productDTO;
                    if (addSuccessfully > 0)
                    {

                        response.Data = productDTO;
                        response.isSuccess = true;
                        response.Message = "Create new Product successfully";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
            
                response.isSuccess = false;
                response.Message = ex.Message;

             
                if (ex.InnerException != null)
                {
                    response.Message = ex.Message + "Inner Exception: " + ex.InnerException.Message;
                }
            }

            return response;
        }


        public async Task<ApiResponse<bool>> DeleteProduct(Guid productID)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.ProductRepository.GetByIdAsync(productID);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "Product does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Product is already deleted";
                    return response;
                }
                _unitOfWork.ProductRepository.SoftRemove(exist);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.isSuccess = true;
                    response.Message = "Product Deleted Successfully";

                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Error deleting Product";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ApiResponse<Pagination<ProductDTO>>> getAllProduct(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<ProductDTO>>();
            var products = await _unitOfWork.ProductRepository.ToPaginationProduct(pageIndex,pageSize);
            var result = _mapper.Map<Pagination<ProductDTO>>(products);
            response.Data = result;
            return response;
        }
        public async Task<ApiResponse<Pagination<ProductDTO>>> getAllProductNotdeleted(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<ProductDTO>>();
            var products = await _unitOfWork.ProductRepository.ToPaginationProduct(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ProductDTO>>(products);
            response.Data = result;
            return response;
        }
        public async Task<ApiResponse<ProductDTO>> GetProductByID(Guid id)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    response.isSuccess = false;
                    response.Message = "not found product";
                }
                else
                {
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    response.isSuccess = true;
                    response.Data = productDTO;
                    response.Message = "Find product successfully";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<ApiResponse<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var existProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productID);
                ValidationResult validationResult = await _createProductvalidator.ValidateAsync(createProductDTO);

                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    if (existProduct != null)
                    {
                        var updateProduct = _mapper.Map(createProductDTO, existProduct);
                        var saveProduct = await _unitOfWork.SaveChangeAsync();
                        if (saveProduct > 0)
                        {
                            response.isSuccess = true;
                            response.Message = "Update product successfully";
                            return response;
                        }
                        else
                        {
                            response.isSuccess = false;
                            response.Message = "Error";
                            return response;
                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.Message = "Product not found";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
 
        public async Task<ApiResponse<IEnumerable<ProductDTO>>> GetAll(int page, List<Guid> CategoryID, string ProductName, int amount, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<ProductDTO>>();

            try
            {
                IEnumerable<ProductDTO> products;

                if (CategoryID == null && string.IsNullOrEmpty(ProductName) && amount <= 0)
                {
                    
                    var pagination = await _unitOfWork.ProductRepository.ToPagination(page, pageSize);
                    response.Data = _mapper.Map<IEnumerable<ProductDTO>>(pagination.Items);
                    response.isSuccess = true;
                    response.Message = "Get all products successfully";
                    return response;
                }


                if (CategoryID != null && CategoryID.Any())
                {
                    products =await  _unitOfWork.ProductRepository.GetProductsByCategoryIDAsync(CategoryID);
                }
                else if (!string.IsNullOrEmpty(ProductName))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByNameAsync(ProductName);
                }
                else if (amount > 0)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByAmountAsync(amount);
                }
                else
                { 
                    var pagination = await _unitOfWork.ProductRepository.ToPaginationProduct(page - 1, pageSize); 
                    response.Data = _mapper.Map<IEnumerable<ProductDTO>>(pagination.Items);
                    response.isSuccess = true;
                    response.Message = "Get all products successfully";
                    return response;
                }

                var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
                response.Data = productDTOs;
                response.isSuccess = true;
                response.Message = "Get all products successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }



    }
}

    