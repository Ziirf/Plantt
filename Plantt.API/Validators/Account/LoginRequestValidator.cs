using FluentValidation;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.DTOs.Requests;
using Plantt.Domain.Interfaces.Repository;

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
                .WithMessage("Username can't be empty.")
                .Must(_unitOfWork.AccountRepository.DoesUsernameExist)
                .WithMessage("No account with that username exists."); ;

            RuleFor(account => account.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty");
        }
    }
}
