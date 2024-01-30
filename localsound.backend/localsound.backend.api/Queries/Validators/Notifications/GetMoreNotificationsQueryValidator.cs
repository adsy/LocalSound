using FluentValidation;
using localsound.backend.api.Queries.Notifications;

namespace localsound.backend.api.Queries.Validators.Notifications
{
    public class GetMoreNotificationsQueryValidator : AbstractValidator<GetMoreNotificationsQuery>
    {
        public GetMoreNotificationsQueryValidator()
        {
            RuleFor(m => m.AppUserId)
                .NotEmpty();

            RuleFor(m => m.MemberId)
                .NotEmpty();

            RuleFor(m => m.Page)
                .NotEmpty();
        }
    }
}
