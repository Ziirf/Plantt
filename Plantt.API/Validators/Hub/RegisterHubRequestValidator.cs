using FluentValidation;
using Plantt.Domain.DTOs.Hub.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;

namespace Plantt.API.Validators.Hub
{
    public class RegisterHubRequestValidator : AbstractValidator<RegisterHubRequest>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterHubRequestValidator(IHttpContextAccessor context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            RuleFor(hub => hub.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be null or empty.")
                .MaximumLength(100)
                .WithMessage("{PropertyName} cannot be longer than 100 characters long.");

            RuleFor(hub => hub.HomeId)
                .Must(IsValidOwnerOfHome)
                .WithMessage("{PropertyName} must belong to the account.");
        }

        private bool IsValidOwnerOfHome(int homeId)
        {
            if (_context.HttpContext?.Items["account"] is AccountEntity account)
            {
                return _unitOfWork.HomeRepository.IsValidOwner(homeId, account.Id);
            }

            return false;
        }
    }
}
