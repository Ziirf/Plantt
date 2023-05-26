using FluentValidation;
using Plantt.Domain.DTOs.Sensor.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;

namespace Plantt.API.Validators.Sensor
{
    public class UpdateSensorRequestValidator : AbstractValidator<UpdateSensorRequest>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSensorRequestValidator(IHttpContextAccessor context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} must not be empty")
                .MaximumLength(100)
                .WithMessage("{PropertyName} cannot be longer than 100 characters long.");

            RuleFor(request => request.HubId)
                .Must(IsValidOwnerOfHub)
                .WithMessage("{PropertyName} must be owned by this account.");

            RuleFor(request => request.MyPlantId)
                .Must(IsValidOwnerOfAccountPlant)
                .WithMessage("{PropertyName} must be owned by this account.");
        }

        private bool IsValidOwnerOfHub(int hubId)
        {
            if (_context.HttpContext?.Items["account"] is AccountEntity account)
            {
                return _unitOfWork.HubRepository.IsValidOwner(hubId, account.Id);
            }

            return false;
        }

        private bool IsValidOwnerOfAccountPlant(int? accountPlantId)
        {
            if (accountPlantId is null)
            {
                return true;
            }

            if (_context.HttpContext?.Items["account"] is AccountEntity account)
            {
                return _unitOfWork.AccountPlantRepository.IsValidOwner((int)accountPlantId, account.Id);
            }

            return false;
        }
    }
}
