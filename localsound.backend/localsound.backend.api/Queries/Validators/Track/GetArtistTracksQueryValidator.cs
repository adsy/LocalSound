using FluentValidation;
using localsound.backend.api.Queries.Track;

namespace localsound.backend.api.Queries.Validators.Track
{
    public class GetArtistTracksQueryValidator : AbstractValidator<GetTracksQuery>
    {
        public GetArtistTracksQueryValidator()
        {
            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("An error occured retrieving artist uploads, please try again...");

            RuleFor(m => m.Page)
                .NotEmpty()
                .WithMessage("An error occured retrieving artist uploads, please try again...");
        }
    }
}
