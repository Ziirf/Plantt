using FluentValidation;
using Plantt.Domain.QueryParams;

namespace Plantt.API.Validators.Plant
{
    public class PlantQueryParamsValidator : AbstractValidator<PlantQueryParams>
    {
        public PlantQueryParamsValidator()
        {
            RuleFor(query => query.PageSize)
                .Must(pageSize => pageSize % 10 == 0)
                .WithMessage("{PropertyName} must be a multiple of 10.")
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be positive number.")
                .LessThanOrEqualTo(100)
                .WithMessage("{PropertyName} must be 100 or less.");

            RuleFor(query => query.Search)
                .MaximumLength(100)
                .WithMessage("{PropertyName} is too long.");
        }
    }
}
