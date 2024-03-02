using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Mime;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ContractViewModels;

namespace eFurnitureProject.API.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)
        {
         var response = await _appointmentService.CreateAppointment(createAppointmentDTO);

            if (response.isSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAppointmentByCustomer(CreateAppointmentDTO createAppointmentDTO , Guid id)
        {
            var result = await _appointmentService.UpdateAppointmentByCustomer(id, createAppointmentDTO);

            if (result.isSuccess)
            {

                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet]
        public async Task<ApiResponse<Pagination<AppointmentDTO>>> GetAllAppointment(int page=0, int amout = 10) =>await _appointmentService.GetAppointmentPaging(page, amout);
        [HttpGet]
        public async Task<ApiResponse<Pagination<AppointmentDTO>>> GetAllAppointmentNotDelete(int page = 0, int amout = 10) => await _appointmentService.GetAppointmentPagingNotDelete(page, amout);
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<AppointmentDTO>>> Filter(int page, String? UserID, string? AppointName, DateTime DateTime, String? Email, int Status, int pageSize)=>
         await _appointmentService.Filter(page, UserID, AppointName, DateTime, Email, Status, pageSize);
        public async  Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByAdmin(Guid appointmentId, List<string> userIds)=> await _appointmentService.UpdateAppointmentByAdmin(appointmentId, userIds);
    }
}
