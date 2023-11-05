using FluentValidation;
using localsound.backend.api.Commands.Track;

namespace localsound.backend.api.Commands.Validators.Track
{
    public class DeleteArtistTrackCommandValidator : AbstractValidator<DeleteArtistTrackCommand>
    {
        public DeleteArtistTrackCommandValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty()
                .WithMessage("User ID cannot be blank/null/empty");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("Member ID cannot be blank/null/empty");

            RuleFor(m => m.TrackId)
                .NotEmpty()
                .WithMessage("Track ID cannot be null");
        }
    }
}
