using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class AppointmentController: BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointnment(CreateAppointmentDTO createAppointmentDTO)
        {
            var appointment = await _appointmentService.CreateAppointment(createAppointmentDTO);
            return Ok(appointment);
        }
    }
}
