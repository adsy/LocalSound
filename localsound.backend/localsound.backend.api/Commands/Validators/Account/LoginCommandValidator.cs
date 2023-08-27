using FluentValidation;
using localsound.backend.api.Commands.Account;

namespace localsound.backend.api.Commands.Validators.Account
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(m => m.LoginDetails)
                .NotEmpty();

            When(m => m.LoginDetails != null, () =>
            {
                RuleFor(m => m.LoginDetails.Email)
                    .NotEmpty();

                RuleFor(m => m.LoginDetails.Password)
                    .NotEmpty();
            });
        }
    }
}
