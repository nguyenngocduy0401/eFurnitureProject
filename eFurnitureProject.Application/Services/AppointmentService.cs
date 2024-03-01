using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
       
        public async Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
                
                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);
               
                await _unitOfWork.AppointmentRepository.AddAsync(appointment);
                await _unitOfWork.SaveChangeAsync();

                var currentID = _claimsService.GetCurrentUserId;
                var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = appointment.Id,
                    UserId = currentID.ToString()
                };
                await _unitOfWork.AppointmentDetailRepository.AddAsync(appointmentDetail);
                await _unitOfWork.SaveChangeAsync();
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

        public async Task<ApiResponse<AppointmentDTO>> UpdateAppointment(Guid ID,CreateAppointmentDTO appointmentDTO)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
                var existingAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(ID);
                if (existingAppointment != null)
                {
                    var updateAppointment = _mapper.Map(appointmentDTO, existingAppointment);
                    var saveAppointment = await _unitOfWork.SaveChangeAsync();
                    if (saveAppointment > 0)
                    {
                        return response;
                    }


                }
            }catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public async Task<ApiResponse<Pagination<AppointmentDTO>>> GetAppointmentPagingNotDelete(int page , int amout)
        {
            var response = new ApiResponse<Pagination<AppointmentDTO>>();
           var appointment =await  _unitOfWork.AppointmentRepository.ToPagination(page, amout);
            var result = _mapper.Map<Pagination<AppointmentDTO>>(appointment);
            
          response.Data = result;
            return response;
        }
       
       
    } }

