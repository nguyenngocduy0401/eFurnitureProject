using eFurnitureProject.Application.ViewModels.ImportViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.ImportValidator
{
    public class UpdateImportValidation : AbstractValidator<UpdateImportDTO>
    {
        public UpdateImportValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255)
               .WithMessage("Length of name must not exceed 255 characters");
            RuleFor(x => x.Status).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(2)
                .WithMessage("Status' value must between 0 and 2");
        }
    }
}
