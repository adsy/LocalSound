using FluentValidation;
using localsound.backend.api.Commands.ArtistPackages;

namespace localsound.backend.api.Commands.Validators.Packages
{
    public class DeleteArtistPackageCommandValidator : AbstractValidator<DeleteArtistPackageCommand>
    {
        public DeleteArtistPackageCommandValidator()
        {
            RuleFor(m => m.AppUserId).NotEmpty().WithMessage("An error occured deleting your package, please try again...");
            RuleFor(m => m.MemberId).NotEmpty().WithMessage("An error occured deleting your package, please try again...");
            RuleFor(m => m.PackageId).NotEmpty().WithMessage("An error occured deleting your package, please try again...");
        }
    }
}
