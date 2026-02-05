using FluentValidation;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Features.Users.Commands.Validators {
    public class AddUserValidator : AbstractValidator<AddUserCommand> {
        public AddUserValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.FirstName).ApplyNameRules();
            RuleFor(x => x.LastName).ApplyNameRules();
            RuleFor(x => x.Email).ApplyEmailRules();
            RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings);
            RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password);
            RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules();
            RuleFor(x => x.UserRole).IsInEnum().WithMessage("Invalid role");
        }
    }
}
