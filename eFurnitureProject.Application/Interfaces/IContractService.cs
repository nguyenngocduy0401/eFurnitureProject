using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ContractViewModels;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IContractService
    {
        public Task<ApiResponse<ContractViewDTO>> CreateContractAsync(CreateContractDTO contract);

        public Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractPagingAsync(int pageIndex = 0, int pageSize = 10);

        public Task<ApiResponse<ContractViewDTO>> UpdateContractAsync(Guid contractId, UpdateContractDTO contract);    

        public Task<ApiResponse<ContractViewDTO>> SoftRemoveContractByIdAsync(Guid contractId);
    }
}
