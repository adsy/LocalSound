using localsound.backend.api.Commands.Notification;
using localsound.backend.api.Queries.Notifications;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace localsound.backend.api.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly IMediator _mediator;

        public NotificationHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CreateNotification(CreateNotificationCommand command)
        {
            var id = Context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var parseResult = Guid.TryParse(id, out var parsedId);
            
            if (parseResult)
            {
                command.CreatorUserId = parsedId;

                var notificationData = await _mediator.Send(command);

                if (notificationData.IsSuccessStatusCode && notificationData.ReturnData != null)
                {
                    await Clients.Group(notificationData.ReturnData.ReceiverUserId.ToString()).SendAsync("ReceiveNotification", notificationData.ReturnData.Notification);
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            var id = Context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var parseResult = Guid.TryParse(id, out var parsedId);

            if (Context?.ConnectionId != null && parseResult)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, parsedId.ToString());

                var result = await _mediator.Send(new GetNotificationsQuery { UserId = parsedId });

                if (result.IsSuccessStatusCode)
                {
                    await Clients.Caller.SendAsync("LoadNotifications", result.ReturnData);
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Context.Abort();

            await base.OnDisconnectedAsync(e);
        }
    }
}
