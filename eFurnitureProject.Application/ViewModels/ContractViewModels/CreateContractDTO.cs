using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingViewModels;
using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class CreateContractDTO
    {
        public string CustomerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Pay { get; set; }
        public CreateOrderProcessingDTO OrderProcessing { get; set; }
    }
}
