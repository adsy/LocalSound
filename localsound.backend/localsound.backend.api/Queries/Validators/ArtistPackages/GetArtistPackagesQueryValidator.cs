using FluentValidation;
using localsound.backend.api.Queries.ArtistPackages;

namespace localsound.backend.api.Queries.Validators.ArtistPackages
{
    public class GetArtistPackagesQueryValidator : AbstractValidator<GetArtistPackagesQuery>
    {
        public GetArtistPackagesQueryValidator()
        {
            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("An error occured getting artist packages, please try again...");
        }
    }
}
