using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Mime;
using System.Net;
using eFurnitureProject.Application.ViewModels.AppointmentDTO;

namespace eFurnitureProject.API.Controllers
{
    public class AppointmentController :BaseController
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController( IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)
        {
            try
            {
                var result = await _appointmentService.CreateAppointment(createAppointmentDTO);
                dynamic reponseObject = new ExpandoObject();
                reponseObject.StatusCode = 0;
                reponseObject.Result = result;
                if (result.isSuccess)
                {
                    reponseObject.StatusCode = HttpStatusCode.OK;
                    return Ok(reponseObject);
                }
                else
                {
                    reponseObject.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(reponseObject);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddStaff(AddStaffDTO addStaffDTO, Guid appointmenrID)
        {
            try
            {
                var result = await _appointmentService.AddStaff(addStaffDTO,appointmenrID);
                dynamic reponseObject = new ExpandoObject();
                reponseObject.StatusCode = 0;
                reponseObject.Result = result;
                if (result.isSuccess)
                {
                    reponseObject.StatusCode = HttpStatusCode.OK;
                    return Ok(reponseObject);
                }
                else
                {
                    reponseObject.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(reponseObject);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
