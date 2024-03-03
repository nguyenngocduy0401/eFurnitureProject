using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<TransactionViewDTO>>> GetTransaction(int pageIndex, int pageSize)
        {
            return await _service.GetTransaction(pageIndex, pageSize);
        }

        [HttpGet]
        public async Task<ApiResponse<TransactionViewDTO>> GetTransactionById(Guid transactionId)
        {
            return await _service.GetTransactionById(transactionId);
        }
    }
}
