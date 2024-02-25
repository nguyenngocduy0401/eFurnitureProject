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
            try
            {
                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);
                await _unitOfWork.AppointmentRepository.AddAsync(appointment);
                var appointmentDTO= _mapper.Map<AppointmentDTO>(appointment);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {

                    response.Data = appointmentDTO;
                    response.isSuccess = true;
                    response.Message = "Create new Appointment successfully";
                    response.Error = string.Empty;
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                response.isSuccess = false;
                response.Error = "An error occurred while creating the Appointment .";
                response.ErrorMessages = new List<string> { ex.Message };

                // If there's an inner exception, include its message as well
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add("Inner Exception: " + ex.InnerException.Message);
                }
            }

            return response;
        }
        public async Task<ApiResponse<AppointmentDetail>> AddStaff(AddStaffDTO addStaffDTO, Guid appointmenrID)
        {
            var response = new ApiResponse<AppointmentDetail>();
            try
            {
                var existAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmenrID);
                if (existAppointment != null)
                {
                    var appointment = _mapper.Map<Appointment>(addStaffDTO);
                    await _unitOfWork.AppointmentRepository.AddAsync(appointment);
                    var appointmentDTO = _mapper.Map<AppointmentDetail>(appointment);
                    var isSuccess = await _unitOfWork.SaveChangeAsync();
                    if (isSuccess > 0)
                    {

                        response.Data = appointmentDTO;
                        response.isSuccess = true;
                        response.Message = "Add successfully";
                        response.Error = string.Empty;
                        return response;
                    }
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Appointment not found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                response.isSuccess = false;
                response.Error = "An error occurred while Add .";
                response.ErrorMessages = new List<string> { ex.Message };

                // If there's an inner exception, include its message as well
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add("Inner Exception: " + ex.InnerException.Message);
                }
            }

            return response;
        }
    }
}
