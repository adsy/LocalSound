using FluentValidation;
using localsound.backend.api.Queries.Track;

namespace localsound.backend.api.Queries.Validators.Track
{
    public class GetUploadTrackDataQueryValidator : AbstractValidator<GetUploadTrackDataQuery>
    {
        public GetUploadTrackDataQueryValidator()
        {
            RuleFor(m => m.AppUserId)
                .NotEmpty()
                .WithMessage("An error occured, please try again...");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("An error occured, please try again...");
        }
    }
}
