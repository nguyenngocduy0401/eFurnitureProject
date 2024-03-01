using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class AppointmentService : IAppointmentService

    {
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public AppointmentService(IClaimsService claimsService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
          
        }
       /* public   AppointmentDetail CreateDetail(CreateAppointmentDetailDTO createAppointmentDetailDTO)
        {
            
            AppointmentDetail appointmennt = _mapper.Map<AppointmentDetail>(createAppointmentDetailDTO);
            appointmennt.UserId = _claimsService.GetCurrentUserId.ToString();
           

       _unitOfWork.AppointmentDetailRepository.AddAsync(_mapper.Map<AppointmentDetail>(createAppointmentDetailDTO));
          _unitOfWork.SaveChangeAsync();
            return appointmennt;
        }*/
        public async Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO, string currentUserId)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
                
                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);

            
                

                
                await _unitOfWork.AppointmentRepository.AddAsync(appointment);
                await _unitOfWork.SaveChangeAsync();

    
                var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

                var appointmentDetail = new AppointmentDetailDTO
                {
                    AppointmentId = appointment.Id,
                    UserId = currentUserId
                };
                response.Data = appointmentDTO;
                response.isSuccess = true;
                response.Message = "Appointment created successfully";
            }
            catch (Exception ex)
            {
     
                response.Data = null;
                response.isSuccess = false;
                response.Message = $"An error occurred while creating the appointment: {ex.Message}";
            }

            return response;
        }
    } }

