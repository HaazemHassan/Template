using FluentValidation;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Features.Users.Commands.Validators {
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand> {
        public UpdateProfileValidator() {
            ApplyValidationRules();
            ApplyCustomValidations();
        }

        private void ApplyValidationRules() {

            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () => {
                RuleFor(x => x.FirstName).ApplyNameRules(true);
            });
            When(x => !string.IsNullOrWhiteSpace(x.LastName), () => {
                RuleFor(x => x.LastName).ApplyNameRules(true);
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () => {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(true);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Address), () => {
                RuleFor(x => x.Address).ApplyAddressRules(true);
            });

        }

        private void ApplyCustomValidations() {
            RuleFor(x => x)
               .Must(HaveAtLeastOneNonNullProperty)
               .WithMessage("Nothing to change. At least one property must be provided for update.");
        }

        private bool HaveAtLeastOneNonNullProperty(UpdateProfileCommand command) {
            return command.FirstName != null ||
                   command.LastName != null ||
                   command.Address != null ||
                   command.PhoneNumber != null;
        }
    }
}
