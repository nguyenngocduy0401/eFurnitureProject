﻿
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class VoucherController : BaseController
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        [Authorize(Roles = AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<VoucherViewDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO) => await _voucherService.CreateVoucherAsync(createVoucherDTO);
        [HttpGet]
        public async Task<ApiResponse<Pagination<VoucherViewDTO>>> GetAllVoucherPaging(int pageIndex, int pageSize) => await _voucherService.GetAllVoucher(pageIndex, pageSize);
        [Authorize(Roles = AppRole.Staff + "," + AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<UpdateVoucherDTO>> UpdateVoucher(CreateVoucherDTO createVoucherDTO, Guid id) => await _voucherService.UpdateVoucher(createVoucherDTO, id);
        [Authorize(Roles = AppRole.Staff + "," + AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<VoucherViewDTO>> GetVouchertoUser([FromQuery] List<string>? userIDs, List<Guid> voucherIds) => await _voucherService.GetVouchertoUser(userIDs, voucherIds);

    }
}