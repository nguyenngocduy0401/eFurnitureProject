using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

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
        public ProductService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMemoryCache memoryCache, IProductRepository productRepository)
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
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                response.isSuccess = false;
                response.Message = ex.Message;

                // If there's an inner exception, include its message as well
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
                if (productDTOs.Count > 0)
                {
                    response.Data = productDTOs;
                    response.isSuccess = true;
                    response.Message = $"Have {productDTOs.Count} product.";
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = $"Have {productDTOs.Count} product.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
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
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
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
                    response.Message = "Product retrieved successfully";
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
                response.Message = ex.Message;
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
                    response.Message = "Product retrieved successfully";
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
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<ApiResponse<IEnumerable<ProductViewDTO>>> GetFilterProductsInPageAsync(int page, int amount, string searchValue)
        {
            var response = new ApiResponse<IEnumerable<ProductViewDTO>>();

            try
            {
                var result = await _unitOfWork.ProductRepository.GetAllAsync();

                var productsDTO = new List<ProductViewDTO>();
                foreach (var product in result)
                {
                    if (!product.IsDeleted)
                    {
                        productsDTO.Add(_mapper.Map<ProductViewDTO>(product));
                    }
                }

                if (productsDTO.Count != 0)
                {
                    response.Data = productsDTO;
                    response.isSuccess = true;
                    response.Message = "";
                }
                else
                {
                    response.isSuccess = true;
                    response.Message = "No record match have found!";
                }
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

        public async Task<ApiResponse<ProductViewDTO>> GetProductDetail(Guid productId)
        {
            var response = new ApiResponse<ProductViewDTO>();

            try
            {
                var result = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
                var productDTO = _mapper.Map<ProductViewDTO>(result);
                if (result != null)
                {
                    response.isSuccess = true;
                    response.Data = productDTO;
                    response.Message = "";
                }
                else
                {
                    response.isSuccess = true;
                    response.Message = "Id has not existed";
                }

                //foreach (var product in result)
                //{
                //    if (!product.IsDeleted)
                //    {
                //        productsDTO.Add(_mapper.Map<ProductViewDTO>(product));
                //    }
                //}
            }
            catch (Exception ex)
            {
                //throw new NotImplementedException();
                Console.WriteLine(ex.ToString());
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;

        }

        public async Task<ApiResponse<IEnumerable<ProductViewDTO>>> GetProductsInPageAsync(int page, int amount)
        {
            var response = new ApiResponse<IEnumerable<ProductViewDTO>>();

            try
            {
                //var result = await _unitOfWork.ProductRepository.GetAllAsync();
                var result = await _unitOfWork.ProductRepository.GetProductPaging(page, amount);

                var productsDTO = new List<ProductViewDTO>();
                foreach (var product in result)
                {
                    if (!product.IsDeleted)
                    {
                        productsDTO.Add(_mapper.Map<ProductViewDTO>(product));
                    }
                }

                if (productsDTO.Count != 0)
                {
                    //throw new Exception("Loi khong xac dinh");
                    response.Data = productsDTO;
                    response.isSuccess = true;
                    response.Message = "";
                }
                else
                {
                    response.isSuccess = true;
                    response.Message = "No results found";
                }
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


        public async Task<ApiResponse<IEnumerable<ProductDTO>>> GetAll(int page, string CategoryName, string ProductName, int amount, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<ProductDTO>>();

            try
            {
                IEnumerable<Product> products;

                // Kiểm tra xem các trường đã được cung cấp hay không
                if (string.IsNullOrEmpty(CategoryName) && string.IsNullOrEmpty(ProductName) && amount <= 0)
                {
                    // Nếu không có thông tin về danh mục, tên sản phẩm hoặc số lượng, thực hiện lấy tất cả sản phẩm
                    products = await _unitOfWork.ProductRepository.GetAllAsync();
                }
                else
                {
                    // Thực hiện tìm kiếm sản phẩm dựa trên các thông tin được cung cấp
                    if (!string.IsNullOrEmpty(CategoryName))
                    {
                        products = await _unitOfWork.ProductRepository.GetProductsByCategoryNameAsync(CategoryName);
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
                        // Nếu không có điều kiện tìm kiếm nào phù hợp, trả về tất cả sản phẩm
                        products = await _unitOfWork.ProductRepository.GetAllAsync();
                    }
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

        public async Task<ApiResponse<IEnumerable<ProductViewDTO>>> FilterProducts(int page, List<Guid>? categoryId, string productName, int amount, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<ProductViewDTO>>();
            try
            {
                // If all filters are null, get all products
                if (categoryId == null && string.IsNullOrEmpty(productName) && amount == 0)
                {
                    var allProducts = await _unitOfWork.ProductRepository.GetAllAsync(); // Assume ProductRepository has GetAllAsync method
                    var productDTOs = _mapper.Map<List<ProductViewDTO>>(allProducts);
                    response.Data = productDTOs;
                }
                else
                {
                    // Apply filters
                    var filteredProducts = await _unitOfWork.ProductRepository.GetAll2(page, categoryId, productName, amount, pageSize);
                    var filteredProductDTOs = _mapper.Map<List<ProductViewDTO>>(filteredProducts);
                    response.Data = filteredProductDTOs;
                }

                response.isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}

    