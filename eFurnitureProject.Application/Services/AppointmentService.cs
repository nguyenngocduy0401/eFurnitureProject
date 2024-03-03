using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<User> _userManager;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public AppointmentService(IClaimsService claimsService, IUnitOfWork unitOfWork, IMapper mapper,UserManager<User> userManager)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
          _userManager= userManager;
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

        public async Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByCustomer(Guid ID,CreateAppointmentDTO appointmentDTO)
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
           var appointment =await  _unitOfWork.AppointmentRepository.GetAppointmentPaging(page, amout);
            var result = _mapper.Map<Pagination<AppointmentDTO>>(appointment);
            
          response.Data = result;
            return response;
        }

        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page, int amout)
        {
            var response = new ApiResponse<Pagination<AppoitmentDetailViewDTO>>();
            var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(page, amout);
            var result = _mapper.Map<Pagination<AppoitmentDetailViewDTO>>(appointment);

            response.Data = result;
            return response;
        }
        public async Task<ApiResponse<IEnumerable<AppointmentDTO>>> Filter(int page, String UserID, string AppointName, DateTime DateTime, String Email, int Status, int pageSize)
        {
            var response = new ApiResponse<IEnumerable<AppointmentDTO>>();

            try
            {
                IEnumerable<Appointment> appointments;

                if (string.IsNullOrEmpty(UserID) && string.IsNullOrEmpty(AppointName) && DateTime == default && string.IsNullOrEmpty(Email) && Status == 0)
                {
                    var pagination = await _unitOfWork.AppointmentRepository.ToPagination(page - 1, pageSize);
                    response.Data = _mapper.Map<IEnumerable<AppointmentDTO>>(pagination.Items);
                    response.isSuccess = true;
                    response.Message = "Get all appointments successfully";
                    return response;
                }

                if (!string.IsNullOrEmpty(UserID))
                {
                    appointments =await _unitOfWork.AppointmentRepository.GetAppointmentsByUserIdAsync(UserID);
                }
                else if (!string.IsNullOrEmpty(AppointName))
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByNameAsync(AppointName);
                }
                else if (DateTime != default)
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByDateTimeAsync(DateTime);
                }
                else if (!string.IsNullOrEmpty(Email))
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByEmailAsync(Email);
                }
                else if (Status != 0)
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByStatusAsync(Status);
                }
                else
                {
                    var pagination = await _unitOfWork.AppointmentRepository.ToPagination(page - 1, pageSize);
                    response.Data = _mapper.Map<IEnumerable<AppointmentDTO>>(pagination.Items);
                    response.isSuccess = true;
                    response.Message = "Get all appointments successfully";
                    return response;
                }

                var appointmentDTOs = _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
                response.Data = appointmentDTOs;
                response.isSuccess = true;
                response.Message = "Get all appointments successfully";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
       
        public async Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
               var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
                if (appointment == null)
                {
                    response.Message = "Appointment not found";
                    return response;
                }

                await _unitOfWork.AppointmentDetailRepository.DeleteByAppointmentIdAsync(appointmentId);
                foreach (var id in staffIds)
                {
                    var appointmentDetail = new AppointmentDetail
                    {
                        AppointmentId = appointment.Id,
                        UserId = id.ToString()
                    };

                    await _unitOfWork.AppointmentDetailRepository.AddAsync(appointmentDetail);

                    await _unitOfWork.SaveChangeAsync();
                }
                response.isSuccess = true;
                response.Message = "Picked staff successfully";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = $"Error picking staff: {ex.Message}";
            }

            return response;
        }
        public async Task<ApiResponse<bool>> UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus newStatus)
        {

            var response = new ApiResponse<bool>();

            try
            {
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
                if (appointment == null)
                {
                    response.Message = "Appointment not found";
                    return response;
                }
                appointment.Status = (int)newStatus;
                await _unitOfWork.SaveChangeAsync();
                response.Data = true;
                response.Message = "Appointment status updated successfully";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = $"Error updating appointment status: {ex.Message}";
            }

            return response;
        }
    }
    } 


