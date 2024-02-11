using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IMessageService
    {
        Task<ServiceResponse> DismissMessageAsync(Guid userId, string memberId, MessageEnum message);
    }
}
