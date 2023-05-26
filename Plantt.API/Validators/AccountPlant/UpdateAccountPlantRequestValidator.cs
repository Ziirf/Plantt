using FluentValidation;
using Plantt.Domain.DTOs.AccountPlant.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;

namespace Plantt.API.Validators.AccountPlant
{
    public class UpdateAccountPlantRequestValidator : AbstractValidator<UpdateAccountPlantRequest>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAccountPlantRequestValidator(IHttpContextAccessor context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            RuleFor(request => request.Name)
                .MaximumLength(100)
                .WithMessage("{PropertyName} cannot be longer than {MaxLength} characters long.");

            RuleFor(request => request.RoomId)
                .Must(IsRoomOwnedByAccount)
                .WithMessage("{PropertyName} must belong to this account");
        }

        private bool IsRoomOwnedByAccount(int room)
        {
            if (_context.HttpContext?.Items["account"] is AccountEntity account)
            {
                return _unitOfWork.RoomRepository.IsValidOwner(room, account.Id);
            }

            return false;
        }
    }
}
