using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService) 
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ApiResponse<TransactionViewDTO>> GetTransactionById(Guid transactionId) =>
            await _transactionService.GetTransactionByIdAsync(transactionId);
    }
}
