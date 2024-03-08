using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using FluentValidation;
namespace eFurnitureProject.API.Validator.AppointmentValidator
{
    public class CreateAppointmentValidation: AbstractValidator<CreateAppointmentDTO>
    {
        public CreateAppointmentValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Date)
                .NotEmpty()
                .Must(BeAValidDate).WithMessage("Date is invalid");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().Matches(@"^0[0-9]{9}$")
                .WithMessage("The phone number must have 10 digits and start with 0!");

            RuleFor(x => x.Email)
                .NotEmpty().NotEmpty().EmailAddress()
                .WithMessage("Email is invalid format!");

            RuleFor(x => x.Time)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Time is greater than zero and less than 10 ")
                .LessThanOrEqualTo(10).WithMessage("Time is greater than zero and less than 10 ");


        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
