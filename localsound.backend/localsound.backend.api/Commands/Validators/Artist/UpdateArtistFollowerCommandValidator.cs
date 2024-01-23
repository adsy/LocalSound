using FluentValidation;
using localsound.backend.api.Commands.Artist;

namespace localsound.backend.api.Commands.Validators.Artist
{
    public class UpdateArtistFollowerCommandValidator : AbstractValidator<UpdateArtistFollowerCommand>
    {
        public UpdateArtistFollowerCommandValidator()
        {
            RuleFor(m => m.ArtistId).NotEmpty().WithMessage("There was an error while updating your following status, please try again.");

            RuleFor(m => m.UserId).NotEmpty().WithMessage("There was an error while updating your following status, please try again.");

            RuleFor(m => m.MemberId).NotEmpty().WithMessage("There was an error while updating your following status, please try again.");
        }
    }
}
