﻿using eFurnitureProject.Application.Commons;
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
        Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByCustomer(Guid ID,CreateAppointmentDTO appointmentDTO);
        Task<ApiResponse<Pagination<AppointmentDTO>>> GetAppointmentPagingNotDelete(int page, int amout);
        Task<ApiResponse<Pagination<AppointmentDTO>>> GetAppointmentPaging(int page, int amout);
        Task<ApiResponse<IEnumerable<AppointmentDTO>>> Filter(int page, String? UserID, string? AppointName, DateTime DateTime, String? Email, int Status, int pageSize);
        Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByAdmin(Guid appointmentId, List<string> userIds);
    }
}