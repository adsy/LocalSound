using FluentValidation;
using localsound.backend.api.Commands.Track;

namespace localsound.backend.api.Commands.Validators.Track
{
    public class CompleteTrackUploadCommandValidator : AbstractValidator<CompleteTrackUploadCommand>
    {
        public CompleteTrackUploadCommandValidator()
        {
            RuleFor(m => m.AppUserId)
                .NotEmpty();

            RuleFor(m => m.PartialTrackId)
                .NotEmpty();

            RuleFor(m => m.MemberId)
                .NotEmpty();
        }
    }
}
