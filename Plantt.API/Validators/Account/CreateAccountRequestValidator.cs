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
                .WithMessage("{PropertyName} can't be empty.")
                .Must(username => username.All(char.IsLetterOrDigit))
                .WithMessage("{PropertyName} must only contain letters and digits.")
                .Must(IsUniqueUsername)
                .WithMessage("{PropertyName} already exists.");

            RuleFor(account => account.Password)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty.")
                .MinimumLength(8)
                .WithMessage("{PropertyName} need to be at least {MinLength} characters long")
                .Must(ContainsBothDigitsAndLetters)
                .WithMessage("{PropertyName} must include both letters and digits");

            RuleFor(account => account.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty.")
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email.");
        }

        private bool IsUniqueUsername(string username)
        {
            return _unitOfWork.AccountRepository.DoesUsernameExist(username) is false;
        }

        private bool ContainsBothDigitsAndLetters(string password)
        {
            return password.Any(char.IsLetter) && password.Any(char.IsDigit);
        }
    }
}
