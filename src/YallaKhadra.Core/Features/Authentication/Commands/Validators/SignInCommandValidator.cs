using FluentValidation;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Authentication.Commands.RequestsModels;
using YallaKhadra.Core.ValidationsRules.Common;

namespace YallaKhadra.Core.Features.Authentication.Commands.Validators {
    public class SignInCommandValidator : AbstractValidator<SignInCommand> {
        public SignInCommandValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();




            When(x => !string.IsNullOrWhiteSpace(x.Email), () => {
                RuleFor(x => x.Email).ApplyEmailRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () => {
                RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings);
            });
        }
    }
}
