using FluentValidation;
using localsound.backend.api.Queries.Notifications;

namespace localsound.backend.api.Queries.Validators.Notifications
{
    public class GetNotificationsQueryValidator : AbstractValidator<GetNotificationsQuery>
    {
        public GetNotificationsQueryValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty();
        }
    }
}
