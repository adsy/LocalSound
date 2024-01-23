using FluentValidation;
using localsound.backend.api.Commands.Packages;

namespace localsound.backend.api.Commands.Validators.Packages
{
    public class CreateArtistPackageCommandValidator : AbstractValidator<CreateArtistPackageCommand>
    {
        public CreateArtistPackageCommandValidator()
        {
            RuleFor(m => m.AppUserId).NotEmpty().WithMessage("An error occured creating your package, please try again...");
            RuleFor(m => m.MemberId).NotEmpty().WithMessage("An error occured creating your package, please try again...");
            RuleFor(m => m.PackageDto).NotEmpty().WithMessage("An error occured creating your package, please try again...");

            When(x => x.PackageDto != null, () =>
            {
                RuleFor(m => m.PackageDto.PackageDescription).NotEmpty().WithMessage("Package description is required to create a package.");
                RuleFor(m => m.PackageDto.PackageName).NotEmpty().WithMessage("Package name is required to create a package.");
                RuleFor(m => m.PackageDto.PackagePrice).NotEmpty().WithMessage("Package price is required to create a package.");
                RuleFor(m => m.PackageDto.PackageEquipment).NotEmpty().WithMessage("Package equipment are required to create a package.");
                RuleFor(m => m.PackageDto.Photos).NotEmpty().WithMessage("Package photos are required to create a package.");
            });
        }
    }
}
