using FluentValidation;
using localsound.backend.api.Commands.Account;

namespace localsound.backend.api.Commands.Validators.Account
{
    public class UpdateProfileImageCommandValidator : AbstractValidator<UpdateAccountImageCommand>
    {
        public UpdateProfileImageCommandValidator()
        {
            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("");
        }
    }
}
