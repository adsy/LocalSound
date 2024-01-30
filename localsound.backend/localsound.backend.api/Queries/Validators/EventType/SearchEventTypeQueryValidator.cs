using FluentValidation;
using localsound.backend.api.Queries.EventType;

namespace localsound.backend.api.Queries.Validators.EventType
{
    public class SearchEventTypeQueryValidator : AbstractValidator<SearchEventTypeQuery>
    {
        public SearchEventTypeQueryValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("There was an error while searching for the event type.");
        }
    }
}
