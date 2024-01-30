using FluentValidation;
using localsound.backend.api.Commands.ArtistPackages;

namespace localsound.backend.api.Commands.Validators.Packages
{
    public class UpdateArtistPackageCommandValidator : AbstractValidator<UpdateArtistPackageCommand>
    {
        public UpdateArtistPackageCommandValidator()
        {
            RuleFor(m => m.AppUserId).NotEmpty().WithMessage("An error occured updating your package, please try again...");
            RuleFor(m => m.MemberId).NotEmpty().WithMessage("An error occured updating your package, please try again...");
            RuleFor(m => m.PackageDto).NotEmpty().WithMessage("An error occured updating your package, please try again...");

            When(x => x.PackageDto != null, () =>
            {
                RuleFor(m => m.PackageDto.PackageDescription).NotEmpty().WithMessage("Package description is required to update your package.");
                RuleFor(m => m.PackageDto.PackageName).NotEmpty().WithMessage("Package name is required to update your package.");
                RuleFor(m => m.PackageDto.PackagePrice).NotEmpty().WithMessage("Package price is required to update your package.");
                RuleFor(m => m.PackageDto.PackageEquipment).NotEmpty().WithMessage("Package equipment are required to update your package.");
            });
        }
    }
}
