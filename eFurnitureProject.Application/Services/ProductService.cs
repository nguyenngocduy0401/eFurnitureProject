using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ProductViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                    response.Data = null;
                    response.isSuccess = true;
                    response.Message = "No record match have found!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.ToString();
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
                    response.Data = null;
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
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.ToString();
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
                    response.Data = null;
                    response.isSuccess = true;
                    response.Message = "No results found";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Data = null;
                response.isSuccess= false;
                response.Message = ex.ToString();
                return response;
            }
            return response;
        }

    }
}
