using FluentValidation;
using Plantt.Domain.DTOs.PlantData.Request;

namespace Plantt.API.Validators.PlantData
{
    public class SendDataRequestValidator : AbstractValidator<SendDataRequest>
    {
        public SendDataRequestValidator()
        {
            RuleFor(request => request.TimeStamp)
                .Must(BeInThePastWithbuffer)
                .WithMessage("{PropertyName} must be in the past");

            RuleFor(request => request.Moisture)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be more than or equal to {ComparisonValue}.")
                .LessThanOrEqualTo(100)
                .WithMessage("{PropertyName} must be less than or equal to {ComparisonValue}.");

            RuleFor(request => request.Humidity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be more than or equal to {ComparisonValue}.")
                .LessThanOrEqualTo(100)
                .WithMessage("{PropertyName} must be less than or equal to {ComparisonValue}.");

            RuleFor(request => request.Lux)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be more than or equal to {ComparisonValue}.");
        }

        private bool BeInThePastWithbuffer(long epochTime)
        {
            var currentEpochTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();
            return epochTime < currentEpochTime;
        }
    }
}
