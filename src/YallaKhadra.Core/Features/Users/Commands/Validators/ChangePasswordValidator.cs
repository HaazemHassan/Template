using FluentValidation;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Extensions.Validations;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Features.Users.Commands.Validators {
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand> {
        public ChangePasswordValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {

            RuleFor(x => x.CurrentPassword).ApplyPasswordRules(passwordSettings, true);
            RuleFor(x => x.NewPassword).ApplyPasswordRules(passwordSettings, true);
            RuleFor(x => x.ConfirmNewPassword).ApplyConfirmPasswordRules(x => x.NewPassword, true);

        }
    }
}
