using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Entities;
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

        public async Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
                var currentUserId = _claimsService.GetCurrentUserId;
                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);

                var appppoimentDetail = new AppointmentDetailDTO
                {
                    AppointmentId = appointment.Id,
                    UserId = currentUserId.ToString()
                };
                _unitOfWork.AppointmentRepository.AddAsync(appointment);
                _unitOfWork.AppointmentDetailRepository.AddAsync(_mapper.Map<AppointmentDetail>(appppoimentDetail));
                _unitOfWork.SaveChangeAsync();
                      var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    } }

