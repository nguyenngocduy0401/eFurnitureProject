using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Application.ViewModels.ResponseViewModel;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class ResponseService : IResponseService
    {
       

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public ResponseService(IResponseRepository responseRepository,IUnitOfWork unitOfWork,IMapper mapper,IClaimsService claimsService)
        {
           _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ResponseDTO>> CreateResponse(CreateResponseDTO responseDTO,Guid feedBackId)
        {
            var response = new ApiResponse<ResponseDTO>();
            try
            {
                var staffId = _claimsService.GetCurrentUserId.ToString();
                var existFeedback = _unitOfWork.FeedbackRepository.GetByIdAsync(feedBackId);
                var feedback = _mapper.Map<Feedback>(existFeedback);
                if (existFeedback == null)
                {
                    response.isSuccess = false;
                    response.Message = "not found";
                }
                else
                {
                    feedback.Status = 2;
                    var responses = _mapper.Map<Response>(responseDTO);
                    responses.Id = feedBackId;
                    responses.StaffId = staffId;
                     await _unitOfWork.ResponseRepository.AddAsync(responses);
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
    }
}
