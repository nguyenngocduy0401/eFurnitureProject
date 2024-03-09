using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDetailViewDTO>> addProductAsysn(AddProductToCartDTO productDTO);
    }
}
