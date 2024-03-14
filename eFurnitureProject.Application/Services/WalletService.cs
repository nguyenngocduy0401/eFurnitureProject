using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public WalletService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<ApiResponse<string>> AddMoneyByUserId(UpdateWalletDTO updateWalletDTO)
        {
            var response = new ApiResponse<string>();
            try 
            {
                
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

        public async Task<ApiResponse<string>> SubtractMoneyByUserId(UpdateWalletDTO updateWalletDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
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
