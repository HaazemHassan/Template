using FluentValidation;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Authentication.Commands.RequestsModels;

namespace YallaKhadra.Core.Features.Authentication.Commands.Validators {
    public class SignInCommandValidator : AbstractValidator<SignInCommand> {
        public SignInCommandValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.Email).ApplyEmailRules(true);
            RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings);
        }
    }
}
