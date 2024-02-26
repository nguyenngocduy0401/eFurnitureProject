using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Utils;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace eFurnitureProject.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly SignInManager<User> _signInManager;
        private readonly AppConfiguration _appConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IValidator<UserRegisterDTO> _validatorRegister;
        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, 
            SignInManager<User> signInManager, AppConfiguration appConfiguration, UserManager<User> userManager,
            IValidator<UserRegisterDTO> validatorRegister, RoleManager<Role> roleManager)
        {

            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _mapper = mapper;
            _appConfiguration = appConfiguration;
            _userManager = userManager;
            _validatorRegister = validatorRegister;
            _roleManager = roleManager;
        }
        public async Task<ApiResponse<string>> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                var result = await _signInManager.PasswordSignInAsync(
                    userLoginDTO.UserName, userLoginDTO.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _unitOfWork.UserRepository.GetUserByUserNameAndPassword(
                        userLoginDTO.UserName, userLoginDTO.Password);
                    var userRole = await _userManager.GetRolesAsync(user);
                    var token = user.GenerateJsonWebToken(
                        _appConfiguration,
                        _appConfiguration.JwtOptions.Secret,
                        _currentTime.GetCurrentTime(),
                        userRole
                        );
                    response.Data = token;
                    response.isSuccess = true;
                    response.Message = "Login is successful!";
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Username or password is not correct!";
                }
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


        public async Task<ApiResponse<UserRegisterDTO>> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            var response = new ApiResponse<UserRegisterDTO>();
            try
            {
                var user = _mapper.Map<User>(userRegisterDTO);
                ValidationResult validationResult = await _validatorRegister.ValidateAsync(userRegisterDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                if (await _unitOfWork.UserRepository.CheckUserNameExisted(userRegisterDTO.Email)) 
                {
                    response.isSuccess = false;
                    response.Message = "UserName is existed!";
                }
                else 
                if (await _unitOfWork.UserRepository.CheckUserNameExisted(userRegisterDTO.UserName)) 
                {
                    response.isSuccess = false;
                    response.Message = "Email is existed!";
                }   
                else
                if (await _unitOfWork.UserRepository.CheckPhoneNumberExisted(userRegisterDTO.PhoneNumber))
                {
                    response.isSuccess = false;
                    response.Message = "PhoneNumber is existed!";
                }
                else {
                   var identityResult = await _userManager.CreateAsync(user, user.PasswordHash);
                    if (identityResult.Succeeded == true)
                    {
                        if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
                        {
                            await _roleManager.CreateAsync(new Role { Name = AppRole.Customer });
                        }
                        await _userManager.AddToRoleAsync(user, AppRole.Customer);
                        response.Data = userRegisterDTO;
                        response.isSuccess = true;
                        response.Message = "Register is successful!";
                    }
                }
            }
            catch (DbException ex) 
            { 
                response.isSuccess = false; 
                response.Message = ex.Message;
            }
            catch(Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
