using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using System;
using System.Collections.Generic;
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
                var result = await _unitOfWork.TransactionRepository.Get(pageIndex, pageSize);
                var viewItems = new List<TransactionViewDTO>();

                foreach (var voucher in result)
                {
                    viewItems.Add(_mapper.Map<TransactionViewDTO>(voucher));
                }

                if (viewItems.Count != 0)
                {
                    response.Data = viewItems;
                    response.isSuccess = true;
                    response.Message = "Success!";
                }
                else
                {
                    response.Data = null;
                    response.isSuccess = true;
                    response.Message = "No reocrd!";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<TransactionViewDTO>> GetTransactionById(Guid transactionId)
        {
            var response = new ApiResponse<TransactionViewDTO>();

            try
            {
                var result = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);


                if (result is not null)
                {
                    response.Data = _mapper.Map<TransactionViewDTO>(result);
                    response.isSuccess = true;
                    response.Message = "Success!";
                }
                else
                {
                    response.Data = null;
                    response.isSuccess = true;
                    response.Message = "No reocrd!";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
