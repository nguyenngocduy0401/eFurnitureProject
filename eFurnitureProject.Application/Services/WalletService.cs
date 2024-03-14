using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<UpdateWalletDTO> _validatorUpdateWallet;
        public WalletService(UserManager<User> userManager, RoleManager<Role> roleManager, IValidator<UpdateWalletDTO> validatorUpdateWallet)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _validatorUpdateWallet = validatorUpdateWallet;
        }
        public async Task<ApiResponse<string>> AddMoneyByUserIdAsync(UpdateWalletDTO updateWalletDTO)
        {
            var response = new ApiResponse<string>();
            try 
            {
                ValidationResult validationResult = await _validatorUpdateWallet.ValidateAsync(updateWalletDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                var user = await _userManager.FindByIdAsync(updateWalletDTO.UserId);

                if (user == null) throw new Exception("Not found user!");

                var checkRole = await _userManager.GetRolesAsync(user);
                if (!checkRole.Contains(AppRole.Customer)) throw new Exception("Only add money for customer!");

                user.Wallet  = user.Wallet + updateWalletDTO.Wallet;
                var isSuccess = await _userManager.UpdateAsync(user);

                if (!isSuccess.Succeeded) throw new Exception("Subtract fail!");
                response.isSuccess = true;
                response.Message = "Successful!";
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

        public async Task<ApiResponse<string>> SubtractMoneyByUserIdAsync(UpdateWalletDTO updateWalletDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                ValidationResult validationResult = await _validatorUpdateWallet.ValidateAsync(updateWalletDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                var user = await _userManager.FindByIdAsync(updateWalletDTO.UserId)
                    ;
                if (user == null) throw new Exception("Not found user!");
                var checkRole = await _userManager.GetRolesAsync(user);

                if (!checkRole.Contains(AppRole.Customer)) throw new Exception("Only subtract money for customer!");

                if (user.Wallet < updateWalletDTO.Wallet) throw new Exception("Insufficient balance in the account");

                user.Wallet = user.Wallet - updateWalletDTO.Wallet;
                var isSuccess = await _userManager.UpdateAsync(user);

                if(!isSuccess.Succeeded) throw new Exception("Subtract fail!");
                response.isSuccess = true;
                response.Message = "Successful!";
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
