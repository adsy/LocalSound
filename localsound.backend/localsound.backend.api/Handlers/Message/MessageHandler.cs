using localsound.backend.api.Commands.Message;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Message
{
    public class MessageHandler : IRequestHandler<DismissMessageCommand, ServiceResponse>
    {
        private readonly IMessageService _messageService;

        public MessageHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<ServiceResponse> Handle(DismissMessageCommand request, CancellationToken cancellationToken)
        {
            return await _messageService.DismissMessageAsync(request.UserId, request.MemberId, request.Message);
        }
    }
}
