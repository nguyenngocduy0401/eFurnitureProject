using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, ICurrentTime currentTime,
            IClaimsService claimsService, IMapper mapper,
            RoleManager<Role> roleManager,UserManager<User> userManager)
        {
            _claimsService = claimsService;
            _currentTime = currentTime; 
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles = AppRole.Admin)]
        public async Task<ApiResponse<UserViewDTO>> GetUserByID(string userID)
        {
            var response = new ApiResponse<UserViewDTO>();
            try
            {
                var user = await _userManager.FindByIdAsync(userID);
                var userData = _mapper.Map<UserViewDTO>(user);
                if (user == null)
                {
                    response.isSuccess = false;
                    response.Message = "Get information successful!";
                    return response;
                }
                response.Data = userData;
                response.isSuccess = true;
                response.Message = "Get information fail!";
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

        [Authorize()]
        public Task<ApiResponse<UserViewDTO>> GetUserInformationByLogin()
        {
            var response = new ApiResponse<UserViewDTO>();
            try
            {
                var userID = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userID);
                var userData = _mapper.Map<UserViewDTO>(user);
                if (user == null)
                {
                    response.isSuccess = false;
                    response.Message = "Get information successful!";
                    return response;
                }
                response.Data = userData;
                response.isSuccess = true;
                response.Message = "Get information fail!";
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
