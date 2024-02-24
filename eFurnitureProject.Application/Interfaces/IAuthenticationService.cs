using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<UserRegisterDTO>> RegisterAsync(UserRegisterDTO userRegisterDTO);
        Task<ApiResponse<string>> LoginAsync(UserLoginDTO userLoginDTO);
    }
}
