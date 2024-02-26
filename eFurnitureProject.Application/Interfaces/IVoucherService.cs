using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IVoucherService
    {
        Task<ApiResponse<CreateVoucherDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO);
        Task<ApiResponse<IEnumerable<VoucherViewDTO>>> GetAllVoucher();
        Task<ApiResponse<UpdateVoucherDTO>> UpdateVoucher(UpdateVoucherDTO updateVoucherDTO);
    }
}
