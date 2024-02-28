using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using eFurnitureProject.Application.Commons;
using AutoMapper.Configuration.Conventions;

namespace eFurnitureProject.API.Controllers
{
    [ApiController]
    public class VoucherController : BaseController
    {
        private readonly IVoucherService _service;

        public VoucherController(IVoucherService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVoucher() {
            var vouchers = await _service.GetAllVoucher();
            return Ok(vouchers);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVoucherPaging(int pageIndex, int pageSize)
        {
            var vouchers = await _service.GetAllVoucherPaging(pageIndex, pageSize);
            return Ok(vouchers);
        }

        [HttpPost]
        public async Task<ApiResponse<CreateVoucherDTO>> CreateVoucher([FromBody] CreateVoucherDTO createVoucherDTO) 
        {
            return await _service.CreateVoucherAsync(createVoucherDTO);
        }

        [HttpPost]
        public async Task<ApiResponse<UpdateVoucherDTO>> UpdateVoucher([FromBody] UpdateVoucherDTO updateVoucherDTO)
        {
            return await _service.UpdateVoucher(updateVoucherDTO);
        }
    }
}
