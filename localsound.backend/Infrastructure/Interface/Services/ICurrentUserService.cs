namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface ICurrentUserService
    {
        Guid AppUserId { get; }
    }
}
