using FluentValidation;
using localsound.backend.api.Queries.Track;

namespace localsound.backend.api.Queries.Validators.Track
{
    public class GetArtistTrackQueryValidator : AbstractValidator<GetArtistTrackQuery>
    {
        public GetArtistTrackQueryValidator()
        {
            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("An error occured retrieving track details, please try again...");
        }
    }
}
