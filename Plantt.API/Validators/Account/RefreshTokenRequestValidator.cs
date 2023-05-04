using FluentValidation;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.DTOs.Requests;

namespace Plantt.API.Validators.Account
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(request => request.AccountPublicId)
                .NotEmpty()
                .WithMessage("AccountPublicId can't be empty");

            RuleFor(request => request.Token)
                .NotEmpty()
                .WithMessage("Token can't be empty");
        }
    }
}