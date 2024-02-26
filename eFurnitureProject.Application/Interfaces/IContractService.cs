using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ContractViewModels;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IContractService
    {
        public Task<ApiResponse<ContractViewModel>> CreateContractAsync(CreateContractViewModel contract);

        public Task<ApiResponse<Pagination<ContractViewModel>>> GetContractPagingsionAsync(int pageIndex = 0, int pageSize = 10);

        public Task<ApiResponse<ContractViewModel>> UpdateContractAsync(Guid contractId, UpdateContractDTO contract);    

        public Task<ApiResponse<ContractViewModel>> SoftRemoveContractByIdAsync(Guid contractId);
    }
}
