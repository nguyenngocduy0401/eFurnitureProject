using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserViewDTO>> GetUserByID(string userID);
        Task<ApiResponse<UserViewDTO>> GetUserInformationByLogin();
    }
}
