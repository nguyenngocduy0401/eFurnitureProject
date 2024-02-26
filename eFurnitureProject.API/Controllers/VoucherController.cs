using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VoucherController : Controller
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

        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherDTO createVoucherDTO) 
        {
            var createVoucher = await _service.CreateVoucherAsync(createVoucherDTO);

            if (!createVoucher.isSuccess)
            {
                return BadRequest(createVoucherDTO);
            }
            else
            {
                return Ok(createVoucherDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVoucher([FromBody] UpdateVoucherDTO updateVoucherDTO)
        {
            var updatevoucher = await _service.UpdateVoucher(updateVoucherDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!updatevoucher.isSuccess)
            {
                return BadRequest(updateVoucherDTO);
            }
            else
            {
                return Ok(updateVoucherDTO);
            }
        }
    }
}
