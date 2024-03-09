using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using System.Data.Common;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace eFurnitureProject.Application.Services
{
    public class ImportService : IImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateImportDTO> _validatorCreateImport;
        private readonly IValidator<CreateImportDetailDTO> _validatorCreateImportDetail;
        private readonly IValidator<UpdateImportDTO> _validatorUpdateImport;

        public ImportService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateImportDTO> validatorCreateImport, IValidator<CreateImportDetailDTO> validatorCreateImportDetail, IValidator<UpdateImportDTO> validatorUpdateImport)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validatorCreateImport = validatorCreateImport;
            _validatorCreateImportDetail = validatorCreateImportDetail;
            _validatorUpdateImport = validatorUpdateImport;
        }

        public async Task<ApiResponse<ImportViewDTO>> CreateImportWithDetailAsync(CreateImportDTO import)
        {
            var response = new ApiResponse<ImportViewDTO>();
            try
            {
                var importObj = _mapper.Map<Import>(import);
                ValidationResult validationCreateImportResult = await _validatorCreateImport.ValidateAsync(import);
                ValidationResult validationCreateImportDetailResult = null;
                foreach (var importdetail in import.ImportDetail)
                {
                    validationCreateImportDetailResult = await _validatorCreateImportDetail.ValidateAsync(importdetail);
                }
                if (!validationCreateImportResult.IsValid || !validationCreateImportResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationCreateImportResult.Errors.Select(error => error.ErrorMessage)
                                                    , ", ", validationCreateImportDetailResult.Errors.Select((error => error.ErrorMessage)));
                    return response;
                }
                else
                {
                    await _unitOfWork.ImportRepository.AddWithDetailAsync(importObj);
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess == true)
                    {
                        response.Data = _mapper.Map<ImportViewDTO>(importObj);
                        response.Message = "Create import with details is successful!";
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

        public async Task<ApiResponse<ImportViewDTO>> UpdateImportAysnc(Guid importId, UpdateImportDTO updateImport)
        {
            var response = new ApiResponse<ImportViewDTO>();
            try
            {
                var existingImport = await _unitOfWork.ImportRepository.GetByIdAsync(importId);
                var importObj = _mapper.Map<Import>(updateImport);
                ValidationResult validationResult = await _validatorUpdateImport.ValidateAsync(updateImport);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                existingImport.Name = importObj.Name;
                existingImport.Status = importObj.Status;
                _unitOfWork.ImportRepository.Update(existingImport);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    if (existingImport.Status == 2)
                    {
                        _unitOfWork.ProductRepository.IncreaseQuantityProductFromImport(existingImport.ImportDetail);
                        if (await _unitOfWork.SaveChangeAsync() > 0)
                        {
                            response.Data = _mapper.Map<ImportViewDTO>(existingImport);
                            response.Message = "Update import and product's quantity are successful!";
                        }
                        return response;
                    }
                    response.Data = _mapper.Map<ImportViewDTO>(existingImport);
                    response.Message = "Update import is successful!";
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
