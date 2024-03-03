using eFurnitureProject.Application.Commons;
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
        Task<IEnumerable<Appointment>> GetAppointmentsByDateTimeAsync(DateTime dateTime);
        Task<IEnumerable<Appointment>> GetAppointmentsByEmailAsync(string email);
        Task<IEnumerable<Appointment>> GetAppointmentsByNameAsync(string appointName);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(int status);
        Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(string userID);
       }
}
