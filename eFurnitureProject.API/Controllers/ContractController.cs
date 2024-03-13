using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class  ContractController : BaseController
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ApiResponse<ContractViewDTO>> CreateContract(CreateContractDTO contract) => await _contractService.CreateContractAsync(contract);

        [HttpGet]
        public async Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractPaging(int pageIndex = 0, int pageSize = 10) => await _contractService.GetContractPagingAsync(pageIndex, pageSize);

        [HttpPut]
        public async Task<ApiResponse<ContractViewDTO>> UpdateContract(Guid contractId, UpdateContractDTO contract) => await _contractService.UpdateContractAsync(contractId, contract);

        [HttpPut]
        public async Task<ApiResponse<ContractViewDTO>> SoftRemoveContractById(Guid contractId) => await _contractService.SoftRemoveContractByIdAsync(contractId);
    }
}