using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        public ProductService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMemoryCache memoryCache,IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _memoryCache = memoryCache;
        }
        public async Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var product = _mapper.Map<Product>(createProductDTO);
               
                await _unitOfWork.ProductRepository.AddAsync(product);
                var addSuccessfully = await _unitOfWork.SaveChangeAsync();
                var productDTO = _mapper.Map<ProductDTO>(product);
                response.Data = productDTO;
                if (addSuccessfully > 0)
                {

                    response.Data = productDTO;
                    response.isSuccess = true;
                    response.Message = "Create new Product successfully";
                    response.Error = string.Empty;
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                response.isSuccess = false;
                response.Error = "An error occurred while creating the product.";
                response.ErrorMessages = new List<string> { ex.Message };

                // If there's an inner exception, include its message as well
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add("Inner Exception: " + ex.InnerException.Message);
                }
            }

            return response;
        }
    

        public async Task<ApiResponse<bool>> DeleteProduct(Guid productID)
        {
            var response = new  ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.ProductRepository.GetByIdAsync(productID);
                if(exist == null)
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
                response.Data = false;
                response.isSuccess = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;

        }

        public async Task<ApiResponse<IEnumerable<ProductDTO>>> getAllProduct()
        {
            var response = new ApiResponse<IEnumerable<ProductDTO>>();
            List<ProductDTO> productDTOs = new List<ProductDTO>();
            try
            {
                List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();
                foreach (var product in products)
                {
                    if (product.IsDeleted == false)
                    {
                        productDTOs.Add(_mapper.Map<ProductDTO>(product));

                    }
                }
                if(productDTOs.Count > 0)
                {
                    response.Data = productDTOs;
                    response.isSuccess = true;
                    response.Message = $"Have {productDTOs.Count} product.";
                    response.Error = "Not error";
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = $"Have {productDTOs.Count} product.";
                    response.Error = "Not have a product";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                response.Error = "Exception";
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
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
                    response.Error = "Product is null";
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
                response.Error = "Exception";
                response.ErrorMessages = new List<string> { ex.Message };
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
                if (existProduct != null) {
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
            }catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                response.ErrorMessages= new List<string> { ex.Message };
                return response;
            }
            return response;
        }
        public async Task<ApiResponse<IEnumerable<ProductDTO>>> SearchProductByCategoryNameAsync(string name)
        {
            var response = new ApiResponse<IEnumerable<ProductDTO>>();

            try
            {
                var products = await _productRepository.GetProductsByCategoryNameAsync(name);
                var productDTOs = new List<ProductDTO>();
                foreach (var product in products)
                {
                    if (!product.IsDeleted)
                    {
                        productDTOs.Add(_mapper.Map<ProductDTO>(product));
                    }
                }
                if (productDTOs.Count != 0)
                {
                    response.isSuccess = true;
                    response.Message = "Produc retrieved successfully";
                    response.Data = productDTOs;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = " Not have Product";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
    

        public async Task<ApiResponse<IEnumerable<ProductDTO>>> SearchProductByNameAsync(string name)
        {
            var response = new ApiResponse<IEnumerable<ProductDTO>>();
            try
            {
                var products = await _unitOfWork.ProductRepository.GetProductsByNameAsync(name);
                var productDTOs = new List<ProductDTO>();
                foreach (var product in products)
                {
                    if (!product.IsDeleted)
                    {
                        productDTOs.Add(_mapper.Map<ProductDTO>(product));
                    }
                }
                if (productDTOs.Count != 0)
                {
                    response.isSuccess = true;
                    response.Message = "Produc retrieved successfully";
                    response.Data = productDTOs;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = " Not have Product";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
