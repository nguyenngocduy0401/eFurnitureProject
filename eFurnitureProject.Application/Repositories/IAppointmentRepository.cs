﻿using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentPaging(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByDateTimeAsync(int pageIndex, int pageSize, DateTime dateTime);
        Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByEmailAsync(int pageIndex, int pageSize, string email);
        Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByNameAsync(int pageIndex, int pageSize, string appointName);
        Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByStatusAsync(int pageIndex, int pageSize,int status);
        Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByUserIdAsync(int pageIndex, int pageSize, string userID);
       }
}
