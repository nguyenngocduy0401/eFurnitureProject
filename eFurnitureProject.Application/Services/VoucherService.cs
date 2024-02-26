using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<VoucherViewDTO>>> GetAllVoucher()
        {
            var response = new ApiResponse<IEnumerable<VoucherViewDTO>>();

            try
            {
                var result = await _unitOfWork.VoucherRepository.GetAllAsync();
                var viewItems = new List<VoucherViewDTO>();

                foreach (var voucher in result)
                {
                    viewItems.Add(_mapper.Map<VoucherViewDTO>(voucher));
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

        public async Task<ApiResponse<CreateVoucherDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO)
        {
            var response = new ApiResponse<CreateVoucherDTO>
            {
                isSuccess = false,
                Message = "Create new voucher failled!"
            };

            if (createVoucherDTO.StartDate > createVoucherDTO.EndDate)
            {
                throw new Exception("Invalid information in voucher!");
            }

            try
            {
                var voucher = _mapper.Map<Voucher>(createVoucherDTO);
                await _unitOfWork.VoucherRepository.AddAsync(voucher);
                await _unitOfWork.SaveChangeAsync();
                _unitOfWork.VoucherRepository.Update(voucher);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.isSuccess = true;
                    response.Data = createVoucherDTO;
                    response.Message = "Create new voucher successfully!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public async Task<ApiResponse<UpdateVoucherDTO>> UpdateVoucher(UpdateVoucherDTO updateVoucherDTO)
        {
            var response = new ApiResponse<UpdateVoucherDTO>
            {
                isSuccess = false,
                Message = "Update new voucher failled!"
            };

            try
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(updateVoucherDTO.Id);
                if (voucher is null)
                {
                    throw new Exception("No voucher found to update!");
                }
                else
                {
                    //voucher.StartDate = updateVoucherDTO.StartDate;
                    //voucher.EndDate = updateVoucherDTO.EndDate;
                    //voucher.Percent = updateVoucherDTO.Percent;
                    voucher.IsDeleted = updateVoucherDTO.IsDeleted;
                    voucher.DeletionDate = updateVoucherDTO.DeletionDate;
                    voucher.DeleteBy = updateVoucherDTO.DeleteBy;
                    voucher.ModificationBy = updateVoucherDTO.ModificationBy;
                    voucher.ModificationDate = DateTime.Now;

                    int update = await _unitOfWork.SaveChangeAsync();
                    if (update > 0)
                    {
                        response.Data = updateVoucherDTO;
                        response.isSuccess = true;
                        response.Message = "Update Succesfully";
                    }
                    else
                    {
                        throw new Exception("Update failled");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }
    }
}
