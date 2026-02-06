using FluentValidation;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Features.Users.Commands.Validators {
    public class RegisterValidator : AbstractValidator<RegisterCommand> {
        public RegisterValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.FirstName).ApplyNameRules(true);
            RuleFor(x => x.LastName).ApplyNameRules(true);
            RuleFor(x => x.Email).ApplyEmailRules(true);
            RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings, true);
            RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password, true);
            RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(true);
        }
    }
}