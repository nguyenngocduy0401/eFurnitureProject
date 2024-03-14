using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<IEnumerable<TransactionViewDTO>>> FilterTransactionByLogin(int pageIndex, int pageSize);
        Task<ApiResponse<TransactionViewDTO>> GetTransactionByIdAsync(Guid transactionId);
    }
}
