using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;   
        }
        [HttpGet]
        public async Task<ApiResponse<UserViewDTO>> GetUserByID(string userID) =>
            await _userService.GetUserByID(userID);

        [HttpGet]
        public async Task<ApiResponse<UserViewDTO>> GetUserInformationByLogin() =>
            await _userService.GetUserInformationByLogin();
    }
}
