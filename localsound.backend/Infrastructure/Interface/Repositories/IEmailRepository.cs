namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IEmailRepository
    {
        Task SendConfirmEmailTokenMessageAsync(string token, string email);
    }
}
