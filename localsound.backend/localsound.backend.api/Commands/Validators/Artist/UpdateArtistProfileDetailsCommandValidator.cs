using FluentValidation;
using localsound.backend.api.Commands.Artist;

namespace localsound.backend.api.Commands.Validators.Artist
{
    public class UpdateArtistProfileDetailsCommandValidator : AbstractValidator<UpdateArtistProfileDetailsCommand>
    {
        public UpdateArtistProfileDetailsCommandValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty()
                .WithMessage("User ID cannot be blank/null/empty");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("Member ID cannot be blank/null/empty");

            RuleFor(m => m.UpdateArtistDto)
                .NotEmpty()
                .WithMessage("Update artist DTO cannot be null");
        }
    }
}
