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

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentPaging(int pageIndex=0, int pageSize=10)
        {
            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();
            var query = (from appointment in _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                         orderby appointment.CreationDate descending
                         select new AppoitmentDetailViewDTO
                         {
                             Id = appointment.Id,
                             Name = appointment.Name,
                             Date = appointment.Date,
                             PhoneNumber = appointment.PhoneNumber,
                             Email = appointment.Email,
                             Status = appointment.Status,
                             Time = appointment.Time,

                             CustomerName = appointment.AppointmentDetail != null ?
                                appointment.AppointmentDetail
                                    .Where(ad => customerUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User)
                                    .FirstOrDefault(user => user != null).UserName
                                : null,

                             StaffName = appointment.AppointmentDetail != null ?
                             appointment.AppointmentDetail
                                    .Where(ad => staffUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User != null ? ad.User.Name : null)
                                    .ToList()
                             : null
                         }) ;

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
             
            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();


            var query = (from appointment in _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                         .Where(a => a.Date >= startDateTime && a.Date < endDateTime)
                         orderby appointment.CreationDate descending
                         select new AppoitmentDetailViewDTO
                         {
                             Id = appointment.Id,
                             Name = appointment.Name,
                             Date = appointment.Date,
                             PhoneNumber = appointment.PhoneNumber,
                             Email = appointment.Email,
                             Status = appointment.Status,
                             Time = appointment.Time,

                             CustomerName = appointment.AppointmentDetail != null ?
                                appointment.AppointmentDetail
                                    .Where(ad => customerUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User)
                                    .FirstOrDefault(user => user != null).UserName
                                : null,

                             StaffName = appointment.AppointmentDetail != null ?
                             appointment.AppointmentDetail
                                    .Where(ad => staffUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User != null ? ad.User.Name : null)
                                    .ToList()
                             : null
                         });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();


            var query = (from appointment in _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                          .Where(p => p.Email.ToLower().Contains(email.ToLower()))
                         orderby appointment.CreationDate descending
                         select new AppoitmentDetailViewDTO
                         {
                             Id = appointment.Id,
                             Name = appointment.Name,
                             Date = appointment.Date,
                             PhoneNumber = appointment.PhoneNumber,
                             Email = appointment.Email,
                             Status = appointment.Status,
                             Time = appointment.Time,

                             CustomerName = appointment.AppointmentDetail != null ?
                                appointment.AppointmentDetail
                                    .Where(ad => customerUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User)
                                    .FirstOrDefault(user => user != null).UserName
                                : null,

                             StaffName = appointment.AppointmentDetail != null ?
                             appointment.AppointmentDetail
                                    .Where(ad => staffUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User != null ? ad.User.Name : null)
                                    .ToList()
                             : null
                         });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();


            var query = (from appointment in _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                          .Where(p => p.Name.ToLower().Contains(appointName.ToLower()))
                         orderby appointment.CreationDate descending
                         select new AppoitmentDetailViewDTO
                         {
                             Id = appointment.Id,
                             Name = appointment.Name,
                             Date = appointment.Date,
                             PhoneNumber = appointment.PhoneNumber,
                             Email = appointment.Email,
                             Status = appointment.Status,
                             Time = appointment.Time,

                             CustomerName = appointment.AppointmentDetail != null ?
                                appointment.AppointmentDetail
                                    .Where(ad => customerUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User)
                                    .FirstOrDefault(user => user != null).UserName
                                : null,

                             StaffName = appointment.AppointmentDetail != null ?
                             appointment.AppointmentDetail
                                    .Where(ad => staffUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User != null ? ad.User.Name : null)
                                    .ToList()
                             : null
                         });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();


            var query = (from appointment in _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                           .Where(p => p.Status == status)
                         orderby appointment.CreationDate descending
                         select new AppoitmentDetailViewDTO
                         {
                             Id = appointment.Id,
                             Name = appointment.Name,
                             Date = appointment.Date,
                             PhoneNumber = appointment.PhoneNumber,
                             Email = appointment.Email,
                             Status = appointment.Status,
                             Time = appointment.Time,

                             CustomerName = appointment.AppointmentDetail != null ?
                                appointment.AppointmentDetail
                                    .Where(ad => customerUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User)
                                    .FirstOrDefault(user => user != null).UserName
                                : null,

                             StaffName = appointment.AppointmentDetail != null ?
                             appointment.AppointmentDetail
                                    .Where(ad => staffUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User != null ? ad.User.Name : null)
                                    .ToList()
                             : null
                         });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
            var customerUserIds = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();

            var staffUserIds = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();


            var query = (from appointment in _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                          .Where(a => a.AppointmentDetail.Any(ad => ad.UserId == userID))
                         orderby appointment.CreationDate descending
                         select new AppoitmentDetailViewDTO
                         {
                             Id = appointment.Id,
                             Name = appointment.Name,
                             Date = appointment.Date,
                             PhoneNumber = appointment.PhoneNumber,
                             Email = appointment.Email,
                             Status = appointment.Status,
                             Time = appointment.Time,

                             CustomerName = appointment.AppointmentDetail != null ?
                                appointment.AppointmentDetail
                                    .Where(ad => customerUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User)
                                    .FirstOrDefault(user => user != null).UserName
                                : null,

                             StaffName = appointment.AppointmentDetail != null ?
                             appointment.AppointmentDetail
                                    .Where(ad => staffUserIds.Contains(ad.UserId))
                                    .Select(ad => ad.User != null ? ad.User.Name : null)
                                    .ToList()
                             : null
                         });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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