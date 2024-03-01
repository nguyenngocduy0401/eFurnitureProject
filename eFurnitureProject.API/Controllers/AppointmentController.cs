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
        public async Task<IActionResult> UpdateAppointmentByAdmin(CreateAppointmentDTO createAppointmentDTO , Guid id)
        {
            var result = await _appointmentService.UpdateAppointment(id, createAppointmentDTO);

            if (result.isSuccess)
            {

                return Ok(result);
            }
            return BadRequest(result);

        }
    }
}
