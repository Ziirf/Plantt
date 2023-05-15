using FluentValidation;
using Plantt.Domain.DTOs.Account.Request;
using Plantt.Domain.Interfaces;

namespace Plantt.API.Validators.Account
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


            RuleFor(account => account.Username)
                .NotEmpty()
                .WithMessage("Username can't be empty.")
                .Must(username => username.All(char.IsLetterOrDigit))
                .WithMessage("Must only contain letters and digits.")
                .Must(username =>
                {
                    return _unitOfWork.AccountRepository.DoesUsernameExist(username) is false;
                })
                .WithMessage("Username already exists.");

            RuleFor(account => account.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty.")
                .MinimumLength(8)
                .WithMessage("Password need to be at least 8 characters long")
                .Must(password => password.Any(char.IsLetter) && password.Any(char.IsDigit))
                .WithMessage("Password must include both letters and digits");

            RuleFor(account => account.Email)
                .NotEmpty()
                .WithMessage("Email can't be empty.")
                .EmailAddress()
                .WithMessage("Email must be a valid email.");
        }
    }
}
