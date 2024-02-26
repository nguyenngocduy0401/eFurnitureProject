﻿using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using System.Data.Common;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace eFurnitureProject.Application.Services
{
    public class ContractService : IContractService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateContractViewModel> _validatorCreateContract;
        private readonly IValidator<UpdateContractDTO> _validatorUpdateContract;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateContractViewModel> validatorCreateContract, IValidator<UpdateContractDTO> validatorUpdateContract)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validatorCreateContract = validatorCreateContract;
            _validatorUpdateContract = validatorUpdateContract;
        }

        public async Task<ApiResponse<ContractViewModel>> CreateContractAsync(CreateContractViewModel contract)
        {
            var response = new ApiResponse<ContractViewModel>();
            try
            {
                var contractObj = _mapper.Map<Contract>(contract);
                ValidationResult validationResult = await _validatorCreateContract.ValidateAsync(contract);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    await _unitOfWork.ContractRepository.AddAsync(contractObj);
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess == true)
                    {
                        response.Data = _mapper.Map<ContractViewModel>(contractObj);
                        response.Message = "Create contract is successful!";
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

        public async Task<ApiResponse<ContractViewModel>> SoftRemoveContractByIdAsync(Guid contractId)
        {
            var response = new ApiResponse<ContractViewModel>();
            try
            {
                var existingContract = await _unitOfWork.ContractRepository.GetByIdAsync(contractId);
                _unitOfWork.ContractRepository.SoftRemove(existingContract);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<ContractViewModel>(existingContract);
                    response.Message = "Remove is successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<Pagination<ContractViewModel>>> GetContractPagingsionAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<ContractViewModel>>();
            var contracts = await _unitOfWork.ContractRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ContractViewModel>>(contracts);
            response.Data = result;
            return response;
        }

        public async Task<ApiResponse<ContractViewModel>> UpdateContractAsync(Guid contractId, UpdateContractDTO contract)
        {
            var response = new ApiResponse<ContractViewModel>();
            try
            {
                var existingContract = await _unitOfWork.ContractRepository.GetByIdAsync(contractId);
                var contractObj = _mapper.Map<Contract>(contract);
                ValidationResult validationResult = await _validatorUpdateContract.ValidateAsync(contract);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    existingContract.Title = contractObj.Title;
                    existingContract.Description = contractObj.Description;
                    existingContract.Value = contractObj.Value;
                    existingContract.Status = contractObj.Status;
                    _unitOfWork.ContractRepository.Update(existingContract);
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess == true)
                    {
                        response.Data = _mapper.Map<ContractViewModel>(existingContract);
                        response.Message = "Update contract is successful!";
                    }
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