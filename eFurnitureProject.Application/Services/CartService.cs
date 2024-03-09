using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartDetailViewDTO>> addProductAsysn(AddProductToCartDTO productDTO)
        {
            ApiResponse<CartDetailViewDTO>? response = new ApiResponse<CartDetailViewDTO>();
            try
            {
                var cartObj = await _unitOfWork.CartRepository.GetCartAsync();
                if(cartObj == null)
                {
                    response.Message = "Not found cart";
                    response.isSuccess = false;
                    return response; 
                }
                if(cartObj.CartDetails == null)
                {
                    cartObj.CartDetails = new List<CartDetail>();
                }
                var cartDetail = _mapper.Map<CartDetail>(productDTO);
                cartDetail.CartId = cartObj.Id;
                await _unitOfWork.CartDetailRepository.AddProductInCartAsync(cartDetail);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<CartDetailViewDTO>(cartDetail);
                    response.Message = "Add product into cart successful!";
                }
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
