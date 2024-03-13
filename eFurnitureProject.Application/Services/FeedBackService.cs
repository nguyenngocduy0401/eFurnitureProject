﻿using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeedBackService(IClaimsService claimsService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<FeedBackDTO>> CreateFeedBack(CreateFeedBackDTO feedBackDTO, Guid feedBackId)
        {
            var response = new ApiResponse<FeedBackDTO>();
            try
            {
                var checkFeedBack = await _unitOfWork.FeedbackRepository.CheckProduct(feedBackId);
                if (!checkFeedBack)
                {
                    response.isSuccess = false;
                    response.Message = "Fail by product";
                }
                else
                {

                    var feedback = _mapper.Map<Feedback>(feedBackDTO);
                    if (feedback.Status == 2)
                    {
                        response.isSuccess = false;
                        response.Message = " FeedBack has responsed ";
                    }
                    else
                    {
                        feedback.Status = 1;
                        feedback.ProductId = feedBackId;
                        await _unitOfWork.FeedbackRepository.AddAsync(feedback);
                        var issuccess = await _unitOfWork.SaveChangeAsync() > 0;
                        if (issuccess)
                        {
                            response.isSuccess = true;
                            response.Message = "Create Successfully";
                            return response;

                        }
                        else
                        {
                            response.isSuccess = false;
                            response.Message = "Create Fail";
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
        public async Task<ApiResponse<Pagination<FeedBackViewDTO>>> GetFeedBackJWT(int pageIndex, int PageSize)
        {
            var response = new ApiResponse<Pagination<FeedBackViewDTO>>();
            try
            {
                var userCurrentID = _claimsService.GetCurrentUserId.ToString();
                var feedbacks = await _unitOfWork.FeedbackRepository.GetFeedBacksByUserID(pageIndex, PageSize, userCurrentID);
                var result = _mapper.Map<Pagination<FeedBackViewDTO>>(feedbacks);

            }
            catch (Exception ex)
            {

                response.Data = null;
                response.isSuccess = false;
                response.Message = $" error : {ex.Message}";
            }

            return response;
        }
        public async Task<ApiResponse<bool>> DeleteFeedBack(Guid id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "FeedBack does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Product is already deleted";
                    return response;
                }
                _unitOfWork.FeedbackRepository.SoftRemove(exist);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                var issuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (issuccess)
                {
                    response.isSuccess = true;
                    response.Message = "Create Successfully";
                    return response;

                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Create Fail";
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
    }
    }
