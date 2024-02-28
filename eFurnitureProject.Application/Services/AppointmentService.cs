using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.AppointmentDTO;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        public AppointmentService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMemoryCache memoryCache, IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _memoryCache = memoryCache;
            _userRepository = userRepository;
        }

       

        public async Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)
        {
            var response = new ApiResponse<AppointmentDTO>();
/*
            try
            {
                // Tạo một đối tượng Appointment từ thông tin trong DTO đầu vào
                var appointment = new Appointment
                {
                    Name = createAppointmentDTO.Name,
                    Date = createAppointmentDTO.Date,
                    PhoneNumber = createAppointmentDTO.PhoneNumber,
                    Email = createAppointmentDTO.Email,
                    Status = createAppointmentDTO.Status,
                    Time = createAppointmentDTO.Time,
                
                };

              *//*  // Lưu thông tin các nhân viên (staff) nếu có
                if (createAppointmentDTO.StaffUserIds != null && createAppointmentDTO.StaffUserIds.Any())
                {
                    foreach (var staffUserId in createAppointmentDTO.StaffUserIds)
                    {
                       
                        appointment.AppointmentDetail.Add(new AppointmentDetail
                        {
                            UserId = staffUserId
                      
                        });
                    }
                }*//*

               
                await _unitOfWork.AppointmentRepository.AddAsync(appointment);
                var isSuccess = await _unitOfWork.SaveChangeAsync();

                if (isSuccess > 0)
                {
                    var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

                    response.Data = appointmentDTO;
                    response.isSuccess = true;
                    response.Message = "Create new Appointment successfully";
                    response.Error = string.Empty;
                    return response;
                }
            }
            catch (Exception ex)
            {
              
                response.isSuccess = false;
                response.Error = "An error occurred while creating the Appointment.";
                response.ErrorMessages = new List<string> { ex.Message };

            
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add("Inner Exception: " + ex.InnerException.Message);
                }
            }*/

            return response;
        }
    }

       
    
}
