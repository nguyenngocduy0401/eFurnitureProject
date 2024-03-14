using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }   

        public async Task<ApiResponse<IEnumerable<TransactionViewDTO>>> GetTransaction(int pageIndex, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<TransactionViewDTO>>();

            try
            {
                

                
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<TransactionViewDTO>> GetTransactionByIdAsync(Guid transactionId)
        {
            var response = new ApiResponse<TransactionViewDTO>();

            try
            {
                var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
                var result = _mapper.Map<TransactionViewDTO>(transaction);

                if (result == null) throw new Exception("Not found!");

                response.Data = result;
                response.isSuccess = true;
                response.Message = "Success!";

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
