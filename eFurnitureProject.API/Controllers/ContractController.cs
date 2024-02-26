using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
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

        [HttpPost]
        public async Task<ApiResponse<ContractViewModel>> CreateContract(CreateContractViewModel contract) => await _contractService.CreateContractAsync(contract);

        [HttpGet]
        public async Task<ApiResponse<Pagination<ContractViewModel>>> GetContractPagingsion(int pageIndex = 0, int pageSize = 10) => await _contractService.GetContractPagingsionAsync(pageIndex, pageSize);

        [HttpPut]
        public async Task<ApiResponse<ContractViewModel>> UpdateContract(Guid contractId, UpdateContractDTO contract) => await _contractService.UpdateContractAsync(contractId, contract);

        [HttpPut]
        public async Task<ApiResponse<ContractViewModel>> SoftRemoveContractById(Guid contractId) => await _contractService.SoftRemoveContractByIdAsync(contractId);
    }
}