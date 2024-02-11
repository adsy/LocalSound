using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IMessageRepository
    {
        Task<ServiceResponse> DismissMessageAsync(Guid userId, MessageEnum message);
    }
}
