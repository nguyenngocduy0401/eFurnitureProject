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
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Enums;

namespace eFurnitureProject.API.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
         _appointmentService = appointmentService;
        
        }
      //  [Authorize(Roles = "CUSTOMER")]
        [HttpPost]
     
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
        [HttpPost]
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
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page=1, int pageSize=10) => await _appointmentService.GetAppointmentPaging(page, pageSize);
              
        [HttpGet]
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter(int page, String? UserID, string? AppointName, DateTime DateTime, String? Email, int Status, int pageSize)
        {
            
           return await _appointmentService.Filter(page, UserID, AppointName, DateTime, Email, Status, pageSize);
        }
        [HttpPost]
        public async Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds) => await _appointmentService.PickStaffForAppointment(appointmentId,  staffIds);
        [HttpPost]
        public async Task<ApiResponse<bool>> UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus newStatus)=> await _appointmentService.UpdateAppointmentStatus(appointmentId, newStatus);
        [HttpDelete]
        public async Task<IActionResult> DeletetAppointment(Guid id)
        {
            var result = await _appointmentService.DeleteAppointment(id);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
