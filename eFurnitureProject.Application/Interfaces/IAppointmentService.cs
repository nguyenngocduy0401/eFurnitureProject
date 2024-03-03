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
        Task<ApiResponse<Pagination<AppointmentDTO>>> GetAppointmentPagingNotDelete(int page, int amout);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page, int amout);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter(int page, String? UserID, string? AppointName, DateTime DateTime, String? Email, int Status, int pageSize);
   
        Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds);
        Task<ApiResponse<bool>> UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus newStatus);
    }
}
