using eFurnitureProject.Application;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public AppointmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService, UserManager<User> userManager) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentPaging(int pageIndex=1, int pageSize=10)
        {
            var query = _dbSet.Include(a => a.AppointmentDetail)
                     .ThenInclude(ad => ad.User)
                     .OrderByDescending(x => x.CreationDate)
                     .Skip((pageIndex - 1) * pageSize)
                     .Take(pageSize)
                     .AsNoTracking();

            var itemCount = await query.CountAsync();
            var items = await query.ToListAsync();

           
            var customerUserIds = await _userManager.GetUsersInRoleAsync("Customer");
            var customerUserId = customerUserIds.FirstOrDefault()?.Id;

            var staffUserIds = await _userManager.GetUsersInRoleAsync("Staff");
            var staffUserIdsList = staffUserIds.Select(user => user.Id).ToList();

            var appointmentDTOs = items.Select(appointment => new AppoitmentDetailViewDTO
            {
                Id = appointment.Id,
                Name = appointment.Name,
                Date = appointment.Date,
                PhoneNumber = appointment.PhoneNumber,
                Email = appointment.Email,
                Status = appointment.Status,
                Time = appointment.Time,
                CustomerName = appointment.AppointmentDetail?.FirstOrDefault(ad => ad.UserId == customerUserId)?.User?.UserName,
                StaffName = appointment.AppointmentDetail?.Where(ad => staffUserIdsList.Contains(ad.UserId)).Select(ad => ad.User?.Name).ToList()
            }).ToList();

            var result = new Pagination<AppoitmentDetailViewDTO>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs,
            };

            return result;
        }


        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateTimeAsync(DateTime dateTime)
        {
            var appointment = await _dbContext.Appointments.Where(u => u.Date ==dateTime).ToListAsync();


            return appointment;
        }

        public async Task<IEnumerable <Appointment>> GetAppointmentsByEmailAsync(string email)
        {
            var appointment = await _dbContext.Appointments.Where(u => u.Email == email).ToListAsync();


            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByNameAsync(string appointName)
        {
            var appointment = await _dbContext.Appointments.Where(u => u.Name == appointName).ToListAsync();


            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(int status)
        {
            var appointment = await _dbContext.Appointments.Where(u => u.Status == status).ToListAsync();


            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(string userID)
        {
            var query = from appointmentDetail in _dbContext.AppointmentDetails
                        join appointment in _dbContext.Appointments on appointmentDetail.AppointmentId equals appointment.Id
                        where appointmentDetail.UserId == userID
                        select appointment;
            return await query.ToListAsync();
        }
        

    }
}