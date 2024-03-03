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

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentPaging(int pageIndex, int pageSize)
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


        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByDateTimeAsync(int pageIndex , int pageSize ,DateTime dateTime)
        {
           
            DateTime startDateTime = dateTime.Date;
            DateTime endDateTime = startDateTime.AddDays(1);

         
            var query = _dbSet.Include(a => a.AppointmentDetail)
                              .ThenInclude(ad => ad.User)
                              .Where(a => a.Date >= startDateTime && a.Date < endDateTime)
                              .OrderByDescending(x => x.CreationDate)
                              .AsNoTracking();

            var itemCount = await query.CountAsync();
            var items = await query.ToListAsync();

            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var appointmentDTOs = items.Select(appointment => new AppoitmentDetailViewDTO
            {
                Id = appointment.Id,
                Name = appointment.Name,
                Date = appointment.Date,
                PhoneNumber = appointment.PhoneNumber,
                Email = appointment.Email,
                Status = appointment.Status,
                Time = appointment.Time,
                CustomerName = appointment.AppointmentDetail?.FirstOrDefault(ad => customerUserIds.Contains(ad.UserId))?.User?.UserName,
                StaffName = appointment.AppointmentDetail?.Where(ad => staffUserIds.Contains(ad.UserId)).Select(ad => ad.User?.Name).ToList()
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

        public async  Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByEmailAsync(int pageIndex, int pageSize, string email)
        {
            var query = _dbSet.Include(a => a.AppointmentDetail)
                             .ThenInclude(ad => ad.User)
                             .Where(p => p.Email.ToLower().Contains(email.ToLower()))
                             .OrderByDescending(x => x.CreationDate)
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

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByNameAsync(int pageIndex, int pageSize, string appointName)
        {
            var query = _dbSet.Include(a => a.AppointmentDetail)
                             .ThenInclude(ad => ad.User)
                             .Where(p => p.Name.ToLower().Contains(appointName.ToLower()))
                             .OrderByDescending(x => x.CreationDate)
                             .AsNoTracking();

            var itemCount = await query.CountAsync();
            var items = await query.ToListAsync();

            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var appointmentDTOs = items.Select(appointment => new AppoitmentDetailViewDTO
            {
                Id = appointment.Id,
                Name = appointment.Name,
                Date = appointment.Date,
                PhoneNumber = appointment.PhoneNumber,
                Email = appointment.Email,
                Status = appointment.Status,
                Time = appointment.Time,
                CustomerName = appointment.AppointmentDetail?.FirstOrDefault(ad => customerUserIds.Contains(ad.UserId))?.User?.UserName,
                StaffName = appointment.AppointmentDetail?.Where(ad => staffUserIds.Contains(ad.UserId)).Select(ad => ad.User?.Name).ToList()
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

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByStatusAsync(int pageIndex, int pageSize, int status )
        {
            var query = _dbSet.Include(a => a.AppointmentDetail)
                            .ThenInclude(ad => ad.User)
                            .Where(p => p.Status == status)
                            .OrderByDescending(x => x.CreationDate)
                            .AsNoTracking();

            var itemCount = await query.CountAsync();
            var items = await query.ToListAsync();

            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var appointmentDTOs = items.Select(appointment => new AppoitmentDetailViewDTO
            {
                Id = appointment.Id,
                Name = appointment.Name,
                Date = appointment.Date,
                PhoneNumber = appointment.PhoneNumber,
                Email = appointment.Email,
                Status = appointment.Status,
                Time = appointment.Time,
                CustomerName = appointment.AppointmentDetail?.FirstOrDefault(ad => customerUserIds.Contains(ad.UserId))?.User?.UserName,
                StaffName = appointment.AppointmentDetail?.Where(ad => staffUserIds.Contains(ad.UserId)).Select(ad => ad.User?.Name).ToList()
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

       

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByUserIdAsync(int pageIndex, int pageSize, string userID)
        {
            var query = _dbSet.Include(a => a.AppointmentDetail)
                            .ThenInclude(ad => ad.User)
                             .Where(a => a.AppointmentDetail.Any(ad => ad.UserId == userID))
                            .OrderByDescending(x => x.CreationDate)
                            .AsNoTracking();

            var itemCount = await query.CountAsync();
            var items = await query.ToListAsync();

            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var appointmentDTOs = items.Select(appointment => new AppoitmentDetailViewDTO
            {
                Id = appointment.Id,
                Name = appointment.Name,
                Date = appointment.Date,
                PhoneNumber = appointment.PhoneNumber,
                Email = appointment.Email,
                Status = appointment.Status,
                Time = appointment.Time,
                CustomerName = appointment.AppointmentDetail?.FirstOrDefault(ad => customerUserIds.Contains(ad.UserId))?.User?.UserName,
                StaffName = appointment.AppointmentDetail?.Where(ad => staffUserIds.Contains(ad.UserId)).Select(ad => ad.User?.Name).ToList()
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
    }
}