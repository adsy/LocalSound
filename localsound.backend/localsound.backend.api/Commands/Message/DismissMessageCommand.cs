using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Message
{
    public class DismissMessageCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public MessageEnum Message { get; set; }
    }
}
