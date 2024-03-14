using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IWalletService
    {
        Task<ApiResponse<string>> SubtractMoneyByUserId(UpdateWalletDTO updateWalletDTO);
        Task<ApiResponse<string>> AddMoneyByUserId(UpdateWalletDTO updateWalletDTO);
    }
}
