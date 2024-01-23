using FluentValidation;
using localsound.backend.api.Commands.Notification;

namespace localsound.backend.api.Commands.Validators.Notification
{
    public class ClickNotificationCommandValidator : AbstractValidator<ClickNotificationCommand>
    {
        public ClickNotificationCommandValidator()
        {
            RuleFor(m => m.AppUserId).NotEmpty().WithMessage("Error occured updating notification ${notificationId} for member:{memberId}");

            RuleFor(m => m.MemberId).NotEmpty().WithMessage("Error occured updating notification ${notificationId} for member:{memberId}");

            RuleFor(m => m.NotificationId).NotEmpty().WithMessage("Error occured updating notification ${notificationId} for member:{memberId}");
        }
    }
}
