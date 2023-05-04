using FluentValidation;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.DTOs.Requests;

namespace Plantt.API.Validators.Account
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        private readonly PlanttDbContext _planttDbContext;

        public LoginRequestValidator(PlanttDbContext planttDbContext)
        {
            _planttDbContext = planttDbContext;

            RuleFor(account => account.Username)
                .NotEmpty()
                .WithMessage("Username can't be empty.")
                .Must(username =>
                {
                    var result = _planttDbContext.Accounts.Any(account => account.Username == username);

                    return result;
                })
                .WithMessage("No account with that username exists."); ;

            RuleFor(account => account.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty");
        }
    }
}
