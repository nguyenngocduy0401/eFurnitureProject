﻿using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IValidator<CreateVoucherDTO> _createVouchervalidator;
       private readonly  UserManager<User> _userManager;
        public VoucherService(UserManager<User> userManager,IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService,IValidator<CreateVoucherDTO>validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _createVouchervalidator = validator;
            _userManager = userManager;
        }

        public async Task<ApiResponse<Pagination<VoucherViewDTO>>> GetAllVoucher(int pageIndex, int pageSize)
        {
            var response = new ApiResponse<Pagination<VoucherViewDTO>>();
          
            var voucher = await _unitOfWork.VoucherRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<VoucherViewDTO>>(voucher);
            response.Data = result;
            return response;
        }

        public async Task<ApiResponse<VoucherViewDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO)
        {
            var response = new ApiResponse<VoucherViewDTO>();
            try
            {
                var voucher = _mapper.Map<Voucher>(createVoucherDTO);
                
                ValidationResult validationResult = await _createVouchervalidator.ValidateAsync(createVoucherDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                await _unitOfWork.VoucherRepository.AddAsync(voucher);
                var issuccess = await _unitOfWork.SaveChangeAsync();
                    if(issuccess>0)
                {
                    response.isSuccess = false;
                    response.Message = "Create Successfully";
                    return response;
                }
                else
                {
                    response.isSuccess = true;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
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

        public async Task<ApiResponse<UpdateVoucherDTO>> UpdateVoucher(CreateVoucherDTO createVoucherDTO,Guid id)
        {
            var response = new ApiResponse<UpdateVoucherDTO>();
           

            try
            {
               
                   var existVoucher = await _unitOfWork.VoucherRepository.GetByIdAsync(id);
                ValidationResult validationResult = await _createVouchervalidator.ValidateAsync(createVoucherDTO);
                if (!validationResult.IsValid)
                {
                   
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    if (existVoucher != null)
                    {
                        var updateVoucher = _mapper.Map(createVoucherDTO, existVoucher);
                        await _unitOfWork.SaveChangeAsync();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public async Task<ApiResponse<VoucherViewDTO>> GetVouchertoUser(List<string>? userIDs,List<Guid> voucherIds)
        {
            var response = new ApiResponse<VoucherViewDTO>();
            try
            {
                foreach (var userID in userIDs)
                {
                    var user = await _userManager.FindByIdAsync(userID);
                    if (user == null)
                    {
                        response.Message = "User not found";
                        response.isSuccess = false;
                        return response;
                    }

                    foreach (var voucherId in voucherIds)
                    {
                        var existVoucher = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId);
                        if (existVoucher == null)
                        {
                            response.Message = "Voucher not found";
                            response.isSuccess = false;
                            return response;
                        }

                        var voucherDetail = new VoucherDetail
                        {
                            UserId = userID,
                            VoucherId = voucherId
                        };
                        await _unitOfWork.VoucherDetailRepository.AddAsync(voucherDetail);
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                response.isSuccess = true;
                response.Message = "Vouchers assigned successfully";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = "User already has a voucher";
            }

            return response;
        }
        public async Task<ApiResponse<bool>> DeleteVoucher(Guid ID)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.VoucherRepository.GetByIdAsync(ID);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "Voucher does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Voucher is already deleted";
                    return response;
                }
                _unitOfWork.VoucherRepository.SoftRemove(exist);
              var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }
        public async Task<ApiResponse<VoucherViewDTO>> SearchVoucherById(Guid id)
        {
            var response= new ApiResponse<VoucherViewDTO>();
            try
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(id);
                if(voucher == null)
                {
                    response.isSuccess = false;
                    response.Message = "Not found";
                }
                else
                {
                    var result =  _mapper.Map<VoucherViewDTO>(voucher);   
                    response.isSuccess = true;
                    response.Data= result;
                   
                }

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public async Task<ApiResponse<List<VoucherViewDTO>>> SearchVoucherByDate(DateTime date)
        {
            var response = new ApiResponse<List<VoucherViewDTO>>();
            try
            {
                var voucher = await _unitOfWork.VoucherRepository.GetVoucherByDateAsync(date);
                if (voucher == null)
                {
                    response.isSuccess = false;
                    response.Message = "not found";
                }
                else
                {


                    var resul = _mapper.Map<List<VoucherViewDTO>>(voucher);
                    response.isSuccess = true;
                    response.Data = resul; }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
    }
    }
