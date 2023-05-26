using FluentValidation;
using Plantt.Domain.DTOs.Hub.Request;

namespace Plantt.API.Validators.Hub
{
    public class LoginHubRequestValidator : AbstractValidator<LoginHubRequest>
    {
        public LoginHubRequestValidator()
        {
            RuleFor(hub => hub.Identity)
                .NotEmpty()
                .WithMessage("{PropertyName} must not be empty");

            RuleFor(hub => hub.Secret)
                .NotEmpty()
                .WithMessage("{PropertyName} must not be empty");
        }
    }
}
