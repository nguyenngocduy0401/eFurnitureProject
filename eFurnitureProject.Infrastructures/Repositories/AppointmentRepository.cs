using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
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
        public AppointmentRepository(
          AppDbContext context,
          ICurrentTime timeService,
          IClaimsService claimsService
,
          UserManager<User> userManager

      )
          : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<Pagination<Appointment>> GetAppointmentPaging(int pageIndex=0, int pageSize=10)
        {
            var query = _dbSet.AsQueryable();


            query = query.Where(x => x.IsDeleted == false);

            var itemCount = await query.CountAsync();
            var items = await query.OrderByDescending(x => x.CreationDate)
                                   .Skip(pageIndex * pageSize)
                                   .Take(pageSize)
                                   .AsNoTracking()
                                   .ToListAsync();

            var result = new Pagination<Appointment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
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
        public async Task<bool> IsUserAdmin(string userId)
        {
            // Giả sử bạn có một cơ chế xác thực để xác định vai trò của người dùng
            // Ví dụ: trong đoạn mã này, giả sử chỉ có vai trò "admin" mới có quyền thực hiện thao tác này

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "staff");
            }
            return false;
        }

    }
}