using FluentValidation;
using localsound.backend.api.Commands.Notification;

namespace localsound.backend.api.Commands.Validators.Notification
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(m => m.CreatorUserId).NotEmpty().WithMessage(x => $"Error occured creating booking created notification for member:{x.ReceiverMemberId}");
            RuleFor(m => m.ReceiverMemberId).NotEmpty().WithMessage(x => $"Error occured creating booking created notification for member:{x.ReceiverMemberId}");
            RuleFor(m => m.Message).NotEmpty().WithMessage(x => $"Error occured creating booking created notification for member:{x.ReceiverMemberId}");
            RuleFor(m => m.RedirectUrl).NotEmpty().WithMessage(x => $"Error occured creating booking created notification for member:{x.ReceiverMemberId}");
        }
    }
}
