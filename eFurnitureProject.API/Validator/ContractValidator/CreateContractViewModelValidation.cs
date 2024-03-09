using eFurnitureProject.Application.ViewModels.ContractViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.ContractValidator
{
    public class CreateContractViewModelValidation : AbstractValidator<CreateContractDTO>
    {
        public CreateContractViewModelValidation()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100)
                .WithMessage("Title length must not exceed 100 characters");
            RuleFor(x => x.Description).MaximumLength(255)
                .WithMessage("Description length must not exceed 255 characters");
            RuleFor(x => x.Value).NotEmpty().GreaterThan(0)
                .WithMessage("Value must greater than 0");
        }
    }
}
