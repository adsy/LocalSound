using FluentValidation;
using localsound.backend.api.Commands.Track;

namespace localsound.backend.api.Commands.Validators.Track
{
    public class UploadTrackChunkCommandValidator : AbstractValidator<UploadTrackChunkCommand>
    {
        public UploadTrackChunkCommandValidator()
        {
            RuleFor(m => m.AppUserId)
                .NotEmpty();

            RuleFor(m => m.PartialTrackId)
                .NotEmpty();

            RuleFor(m => m.ChunkId)
                .NotEmpty();

            RuleFor(m => m.File)
                .NotEmpty();
        }
    }
}
