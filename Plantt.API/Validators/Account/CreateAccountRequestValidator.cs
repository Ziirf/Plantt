using FluentValidation;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.DTOs.Requests;

namespace Plantt.API.Validators.Account
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        private readonly PlanttDbContext _planttDbContext;

        public CreateAccountRequestValidator(PlanttDbContext planttDbContext)
        {
            _planttDbContext = planttDbContext;

            RuleFor(account => account.Username)
                .NotEmpty()
                .WithMessage("Username can't be empty.")
                .Must(username =>
                {
                    var result = _planttDbContext.Accounts.Any(account => account.Username == username);

                    return !result;
                })
                .WithMessage("Username already exists.");

            RuleFor(account => account.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty.")
                .MinimumLength(8)
                .WithMessage("Password need to be at least 8 characters long")
                .Must(password => password.Any(char.IsLetter) && password.Any(char.IsDigit))
                .WithMessage("Password must include both letters and numbers");

            RuleFor(account => account.Email)
                .NotEmpty()
                .WithMessage("Email can't be empty.")
                .EmailAddress()
                .WithMessage("Email must be a valid email.");
        }
    }
}
