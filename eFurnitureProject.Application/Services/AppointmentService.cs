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
                    return response;
                }
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
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter(FilterAppointmentDTO filterAppointment,DateTime date,int status) 
        {

            var response = new ApiResponse<Pagination<AppoitmentDetailViewDTO>>();

            try
            {
                Pagination<AppoitmentDetailViewDTO> appointments;

                if (string.IsNullOrEmpty(filterAppointment.search) && date == default && status == 0)
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(filterAppointment.pageIndex, filterAppointment.pageSize);
                }
                else if (!string.IsNullOrEmpty(filterAppointment.search))
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentByFilter(filterAppointment.search, filterAppointment.pageIndex, filterAppointment.pageSize);
                }
                else if (date != default)
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByDateTimeAsync(filterAppointment.pageIndex, filterAppointment.pageSize, date);
                }
                else if (status != 0)
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentsByStatusAsync(filterAppointment.pageIndex, filterAppointment.pageSize, status);
                }
                else
                {
                    appointments = await _unitOfWork.AppointmentRepository.GetAppointmentPaging(filterAppointment.pageIndex, filterAppointment.pageSize);
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


      /*  public async Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds)
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
                var existingAppointmentDetails = await _unitOfWork.AppointmentDetailRepository.GetByAppointmentIdAsync(appointmentId);

                await _unitOfWork.AppointmentDetailRepository.DeleteByAppointmentIdAsync(appointmentId);
                foreach (var id in staffIds)
                {
                    var existingAppointmentDetail = existingAppointmentDetails.FirstOrDefault(ad => ad.UserId == id.ToString());

                    if (existingAppointmentDetail != null)
                    {
                        existingAppointmentDetail.IsDeleted = false;
                        await _unitOfWork.AppointmentDetailRepository.UpdateAsync(existingAppointmentDetail);
                    }

                    else
                    {
                        var appointmentDetail = new AppointmentDetail
                        {
                            AppointmentId = appointment.Id,
                            UserId = id.ToString()
                        };

                        await _unitOfWork.AppointmentDetailRepository.AddAsync(appointmentDetail);
                    }
                    await _unitOfWork.SaveChangeAsync();
                }

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = $"Error picking staff: {ex.Message}";
            }

            return response;
        }*/
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
                }
            catch (Exception ex)
            {
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
        public async Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(Guid appointmentId, List<string> staffIds)
        {
            var response = new ApiResponse<AppointmentDTO>();
            try
            {
                // Lấy thông tin cuộc hẹn
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
                if (appointment == null)
                {
                    response.Message = "Appointment not found";
                    return response;
                }

            
                if (appointment.Status != (int)AppointmentStatus.Processing && appointment.Status != (int)AppointmentStatus.Pending)
                {
                    response.Message = "Cannot pick staff for appointment: Appointment status is not Processing or Pending";
                    return response;
                }

                // Kiểm tra xem staffIDs có rỗng không
                if (staffIds == null || !staffIds.Any())
                {
                    response.Message = "No staff selected for appointment";
                    return response;
                }

                // Lấy thông tin chi tiết cuộc hẹn
                var existingDetails = await _unitOfWork.AppointmentDetailRepository.GetByAppointmentIdAsync(appointmentId);

                // Kiểm tra xung đột thời gian cho từng nhân viên đã chọn
                foreach (var detail in existingDetails)
                {
                    if (IsTimeConflict(detail, existingDetails))
                    {
                        response.Message = "Schedule conflict: One of the selected staff has a schedule conflict";
                        return response;
                    }
                }

                // Thêm các chi tiết cuộc hẹn mới cho từng nhân viên đã chọn
                foreach (var staffId in staffIds)
                {
                    var existingDetail = existingDetails.FirstOrDefault(d => d.UserId == staffId);

                    if (existingDetail != null)
                    {
                        existingDetail.IsDeleted = false;
                        await _unitOfWork.AppointmentDetailRepository.UpdateAsync(existingDetail);
                    }
                    else
                    {
                        var newDetail = new AppointmentDetail
                        {
                            AppointmentId = appointmentId,
                            UserId = staffId
                        };
                        await _unitOfWork.AppointmentDetailRepository.AddAsync(newDetail);
                    }
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.SaveChangeAsync();

                response.isSuccess = true;
                response.Message = "Staff picked for appointment successfully";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = $"Error picking staff: {ex.Message}";
            }

            return response;
        }

        private bool IsTimeConflict(AppointmentDetail appointmentDetail, IEnumerable<AppointmentDetail> existingDetails)
        {
            // Lấy danh sách các chi tiết cuộc hẹn khác có cùng UserId
            var conflictingDetails = existingDetails.Where(ad => ad.UserId == appointmentDetail.UserId && ad.IsDeleted == false);

            // Kiểm tra xem có trùng giờ với các cuộc hẹn khác hoặc cách ít nhất 5 giờ không
            foreach (var detail in conflictingDetails)
            {
                var timeDiff = Math.Abs((detail.Appointment.Date - appointmentDetail.Appointment.Date).TotalHours);

                if (timeDiff < 5)
                {
                    return true;
                }
            }

            return false;
        }
    }
    }
    
    


