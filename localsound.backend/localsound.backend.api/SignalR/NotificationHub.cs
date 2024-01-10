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
