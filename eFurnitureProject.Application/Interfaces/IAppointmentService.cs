using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Enums;
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
        Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByCustomer(Guid ID,CreateAppointmentDTO appointmentDTO);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page, int amout);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter(FilterAppointmentDTO filterAppointment, DateTime date, int status);
        Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds);
        Task<ApiResponse<bool>> UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus newStatus);
        Task<ApiResponse<bool>> DeleteAppointment(Guid ID);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentByJWT(int pageIndex, int pageSize);
    }
}
