using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class AppointmentDetailRepository : IAppointmentDetailRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public AppointmentDetailRepository(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task AddAsync(AppointmentDetail appointmentDetail)
        {
            await _dbContext.AppointmentDetails.AddAsync(appointmentDetail);
        }
        public async Task DeleteByAppointmentIdAsync(Guid appointmentId)
        {


            var appointmentDetails = await _dbContext.AppointmentDetails
         .Where(ad => ad.AppointmentId == appointmentId) // Lọc theo AppointmentId
         .Join(_dbContext.UserRoles.Where(ur => ur.RoleId == "3"), // Join với bảng UserRoles để kiểm tra RoleId
               ad => ad.UserId,
               ur => ur.UserId,
               (ad, ur) => ad) // Chỉ lấy AppointmentDetails có UserId có RoleId = 3
         .ToListAsync();

            appointmentDetails.ForEach(ad => ad.IsDeleted = true); // Gán IsDeleted = true cho các AppointmentDetail phù hợp

            await _dbContext.SaveChangesAsync();


        }
    }
}
