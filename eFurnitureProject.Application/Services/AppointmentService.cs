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
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eFurnitureProject.Application.Utils;
using eFurnitureProject.Application.ViewModels.UserViewModels;

using System.Data.Common;

using System.Globalization;


namespace eFurnitureProject.Application.Services
{
    public class AppointmentService : IAppointmentService

    {
        private readonly UserManager<User> _userManager;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAppointmentDTO>   _createAppointmentValidator;
        public AppointmentService(IValidator<CreateAppointmentDTO> validatorCreateAppointment, IClaimsService claimsService, IUnitOfWork unitOfWork, IMapper mapper,UserManager<User> userManager)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
          _userManager= userManager;
            _createAppointmentValidator = validatorCreateAppointment;
        }
       
        public async Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {

                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);
                appointment.Status = 2;
                ValidationResult validationResult = await _createAppointmentValidator.ValidateAsync(createAppointmentDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {

                   
                    var currentID = _claimsService.GetCurrentUserId;
                    if (currentID == Guid.Empty)
                    {
                        response.isSuccess = false;
                        response.Message = "Register please";
                    }
                    else
                    {
                        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
                        await _unitOfWork.SaveChangeAsync();

                        var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

                        var appointmentDetail = new AppointmentDetail
                        {
                            AppointmentId = appointment.Id,
                            UserId = currentID.ToString()
                        };
                        await _unitOfWork.AppointmentDetailRepository.AddAsync(appointmentDetail);
                        var issuccess = await _unitOfWork.SaveChangeAsync()>0;
                        if (issuccess )
                        {
                            response.isSuccess = true;
                            response.Message = "Create Successfully";
                            return response;
                          
                        }
                        else
                        {
                            response.isSuccess = false;
                            response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                            return response;
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
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
                    ValidationResult validationResult = await _createAppointmentValidator.ValidateAsync(appointmentDTO);
                    if (!validationResult.IsValid)
                    {
                        response.isSuccess = false;
                        response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                        return response;
                    }
                    else
                    {
                        var saveAppointment = await _unitOfWork.SaveChangeAsync();
                        if (saveAppointment > 0)
                        {
                            response.isSuccess = true;
                            return response;
                        }


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
      
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page, int amout)
        {
            var response = new ApiResponse<Pagination<AppoitmentDetailViewDTO>>();
            var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(page, amout);
            var result = _mapper.Map<Pagination<AppoitmentDetailViewDTO>>(appointment);

            response.Data = result;
            return response;
        }
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter(FilterAppointmentDTO filterAppointment,string date,int status) 
        {

            var response = new ApiResponse<Pagination<AppoitmentDetailViewDTO>>();

            try
            {

                Pagination<AppoitmentDetailViewDTO> appointments;
                DateTime parsedDate = DateTime.MinValue; // Gán giá trị mặc định cho parsedDate

                if (string.IsNullOrEmpty(filterAppointment.search) && string.IsNullOrEmpty(date) && status == 0)
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(filterAppointment.pageIndex, filterAppointment.pageSize);
                }
                
                
                    if (!string.IsNullOrEmpty(date) && !DateTime.TryParse(date, out parsedDate))
                    {
                        response.isSuccess = false;
                        response.Message = "Invalid date format";
                        return response;
                    }
                    if (string.IsNullOrEmpty(filterAppointment.search) && string.IsNullOrWhiteSpace(date) && status == 0)
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(filterAppointment.pageIndex, filterAppointment.pageSize);
                    }
                    else if (!string.IsNullOrEmpty(filterAppointment.search) && date != default && status != 0)
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsBySearchDateAndStatusAsync(filterAppointment.search, parsedDate, status, filterAppointment.pageIndex, filterAppointment.pageSize);
                    }
                    else if (string.IsNullOrEmpty(filterAppointment.search) && date != default && status != 0)
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByDateAndStatusAsync(parsedDate, status, filterAppointment.pageIndex, filterAppointment.pageSize);
                    }
                    else if (!string.IsNullOrEmpty(filterAppointment.search) && status != 0 && string.IsNullOrWhiteSpace(date))
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsBySearchAndStatusAsync(filterAppointment.search, status, filterAppointment.pageIndex, filterAppointment.pageSize);
                    }
                    else if (!string.IsNullOrEmpty(filterAppointment.search) && status == 0 && string.IsNullOrWhiteSpace(date))
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentByFilter(filterAppointment.search, filterAppointment.pageIndex, filterAppointment.pageSize);
                    }
                    else if (date != default && string.IsNullOrEmpty(filterAppointment.search) && status == 0)
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByDateTimeAsync(filterAppointment.pageIndex, filterAppointment.pageSize, parsedDate);
                    }
                    else if (status != 0 && string.IsNullOrWhiteSpace(date) && string.IsNullOrEmpty(filterAppointment.search))
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByStatusAsync(filterAppointment.pageIndex, filterAppointment.pageSize, status);
                    }
                    else
                    {
                        appointments = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(filterAppointment.pageIndex, filterAppointment.pageSize);
                        response.isSuccess = true;
                        response.Message = "Get all appointments successfully";
                    }

                    var appointmentsDTO = _mapper.Map<Pagination<AppoitmentDetailViewDTO>>(appointments);
                    response.Data = appointmentsDTO;
                    response.isSuccess = true;
                    response.Message = "Get all appointments successfully";
                }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.isSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
               
                var appoiment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
                var time = ParseTime(appoiment.Time).Hours;
               foreach (var staffId in staffIds)
                {
                    var appointmentOfStaffs = await _unitOfWork.AppointmentDetailRepository.GetByAppointmentByStaffIdAsync(staffId);
                    foreach(var appointmentOfStaff in appointmentOfStaffs)
                    {
                        var timeCheck1 = ParseTime(appointmentOfStaff.Appointment.Time).Hours - time;
                        var timeCheck2 = time - ParseTime(appointmentOfStaff.Appointment.Time).Hours;

                    }
                }
                

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
                    response.isSuccess = false;
                    response.Message = "Appointment not found";
                    return response;
                }
                appointment.Status = (int)newStatus;
               var isSuccess= await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.isSuccess = true;
                    response.Message = "Update Successfully";
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Update UnSuccessfully";
                    return response;
                }
                }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Data = false;
                response.Message = $"Error updating appointment status: {ex.Message}";
            }

            return response;
        }
        public async Task<ApiResponse<bool>> DeleteAppointment(Guid ID)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.AppointmentRepository.GetByIdAsync(ID);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "Product does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Product is already deleted";
                    return response;
                }
                _unitOfWork.AppointmentRepository.SoftRemove(exist);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.isSuccess = true;
                    response.Message = "Delete Successfully";
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Delete UnSuccessfully";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentByJWT(int pageIndex,int pageSize)
        {
            var response = new ApiResponse<Pagination<AppoitmentDetailViewDTO>>();
            var currentUserID = _claimsService.GetCurrentUserId.ToString();
            var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentsByUserID(pageIndex, pageSize,currentUserID);
            var result = _mapper.Map<Pagination<AppoitmentDetailViewDTO>>(appointment);
            response.Data = result;
            return response;
        }
        private TimeSpan ParseTime(string? time)
        {
            if (string.IsNullOrEmpty(time))
            {
               
                return TimeSpan.Zero;
            }

            if (DateTime.TryParseExact(time, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTime))
            {
                return parsedTime.TimeOfDay;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }
        private int ParseTimetoInt(string timeString)
        {
            DateTime appointmentTime = DateTime.Parse(timeString);
            return appointmentTime.Hour;
        }
    }
    }
    
    


