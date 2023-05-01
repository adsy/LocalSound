using FluentValidation;
using localsound.backend.api.Commands.Account;

namespace localsound.backend.api.Commands.Validators.Account
{
    public class NonArtistLoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public NonArtistLoginCommandValidator()
        {
            RuleFor(m => m.UserDetails)
                .NotEmpty();

            When(m => m.UserDetails != null, () =>
            {
                RuleFor(m => m.UserDetails.Email)
                    .NotEmpty();

                RuleFor(m => m.UserDetails.Password)
                    .NotEmpty();
            });
        }
    }
}
