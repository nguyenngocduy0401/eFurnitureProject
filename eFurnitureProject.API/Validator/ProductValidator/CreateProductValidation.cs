using eFurnitureProject.Application.ViewModels.ProductDTO;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace eFurnitureProject.API.Validator.ProductValidator
{
    public class CreateProductValidation:AbstractValidator<CreateProductDTO>
    {
        public CreateProductValidation()
        {
            
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required");

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required");

                RuleFor(x => x.Image)
                    .NotEmpty().WithMessage("Image is required");

                RuleFor(x => x.InventoryQuantity)
                    .NotEmpty().WithMessage("InventoryQuantity is required")
                    .GreaterThan(0).WithMessage("InventoryQuantity must be greater than 0");

                RuleFor(x => x.Status)
                    .NotEmpty().WithMessage("Status is required");

                RuleFor(x => x.CategoryId)
                    .NotEmpty().WithMessage("CategoryId is required");
            
        }
    }
}
