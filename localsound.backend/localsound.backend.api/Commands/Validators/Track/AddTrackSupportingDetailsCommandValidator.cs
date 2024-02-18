using FluentValidation;
using localsound.backend.api.Commands.Track;

namespace localsound.backend.api.Commands.Validators.Track
{
    public class AddTrackSupportingDetailsCommandValidator : AbstractValidator<AddTrackSupportingDetailsCommand>
    {
        public AddTrackSupportingDetailsCommandValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty()
                .WithMessage("User ID cannot be blank/null/empty");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("Member ID cannot be blank/null/empty");

            RuleFor(m => m.TrackData)
                .NotEmpty()
                .WithMessage("Track data DTO cannot be null");
        }
    }
}
