using FluentValidation;
using Plantt.Domain.DTOs.Room.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;

namespace Plantt.API.Validators.Room
{
    public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoomRequestValidator(IHttpContextAccessor context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            RuleFor(room => room.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be null or empty.");

            RuleFor(room => room.SunlightLevel)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be null or empty.")
                .LessThanOrEqualTo(10)
                .WithMessage("{PropertyName} must be less than or equal to {ComparisonValue}.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be more than or equal to {ComparisonValue}.");

            RuleFor(room => room.HomeId)
                .NotEmpty()
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
