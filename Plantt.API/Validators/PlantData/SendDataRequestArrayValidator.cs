using FluentValidation;
using Plantt.Domain.DTOs.PlantData.Request;

namespace Plantt.API.Validators.PlantData
{
    public class SendDataRequestArrayValidator : AbstractValidator<List<SendDataRequest>>
    {
        public SendDataRequestArrayValidator()
        {
            RuleForEach(list => list).SetValidator(new SendDataRequestValidator());
        }
    }
}
