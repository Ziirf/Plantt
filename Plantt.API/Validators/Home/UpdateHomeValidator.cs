using FluentValidation;
using Plantt.Domain.DTOs.Home.Request;

namespace Plantt.API.Validators.Home
{
    public class UpdateHomeValidator : AbstractValidator<UpdateHomeRequest>
    {
        public UpdateHomeValidator(IHttpContextAccessor context)
        {
            RuleFor(home => home.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be null or empty.")
                .MaximumLength(100)
                .WithMessage("{PropertyName} cannot be longer than {MaxLength} characters long.");
        }
    }
}
