using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace localsound.backend.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            AppUserId = Guid.TryParse(httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
        }
        public Guid AppUserId { get; }
    }
}
