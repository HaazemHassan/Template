using FluentValidation;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Users.Queries.Models;

namespace YallaKhadra.Core.Features.Users.Queries.Validators {
    public class CheckEmailAvailabilityValidator : AbstractValidator<CheckEmailAvailabilityQuery> {
        public CheckEmailAvailabilityValidator() {
            ApplyValidationRules();
        }

        public void ApplyValidationRules() {
            RuleFor(x => x.Email).ApplyEmailRules(isRequired: true);
        }
    }
}
