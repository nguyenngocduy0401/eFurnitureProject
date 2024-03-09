﻿using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using System;
using System.Collections.Generic;
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
        public FeedBackService(IClaimsService claimsService, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    
        public async Task<ApiResponse<FeedBackDTO>> CreateFeedBack(CreateFeedBackDTO feedBackDTO,Guid productId)
        {
            var response = new ApiResponse<FeedBackDTO>();
            try
            {
                var checkProduct = await _unitOfWork.FeedbackRepository.CheckProduct(productId);
                if (!checkProduct )
                {
                    response.isSuccess = false;
                    response.Message="Fail by product";
                }
                else
                {

                    var feedback = _mapper.Map<Feedback>(feedBackDTO);
                    feedback.ProductId = productId;
                    await _unitOfWork.FeedbackRepository.AddAsync(feedback);
                    await _unitOfWork.SaveChangeAsync();
                    return response;
                }


            }catch (Exception ex)
            {

                response.Data = null;
                response.isSuccess = false;
                response.Message = $"An error occurred while creating the appointment: {ex.Message}";
            }

            return response;
        }
    }
}
