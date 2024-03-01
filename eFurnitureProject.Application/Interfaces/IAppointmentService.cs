using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO);
        Task<ApiResponse<AppointmentDTO>> UpdateAppointment(Guid ID,CreateAppointmentDTO appointmentDTO);

    }
}
