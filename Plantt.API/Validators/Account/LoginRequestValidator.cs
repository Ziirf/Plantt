using FluentValidation;
using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.Interfaces;

namespace Plantt.API.Validators.Account
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(account => account.Username)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty.")
                .Must(_unitOfWork.AccountRepository.DoesUsernameExist)
                .WithMessage("No account with that username exists.");

            RuleFor(account => account.Password)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");
        }
    }
}
