using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentDTO;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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
        Task<ApiResponse<AppointmentDetail>> AddStaff(AddStaffDTO addStaffDTO,Guid appointmenrID);
    }
}
