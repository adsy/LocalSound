using FluentValidation;
using localsound.backend.api.Queries.Account;

namespace localsound.backend.api.Queries.Validators
{
    public class GetProfileDataQueryValidator : AbstractValidator<GetProfileDataQuery>
    {
        public GetProfileDataQueryValidator()
        {
            RuleFor(m => m.ProfileUrl)
                .NotEmpty();
        }
    }
}
