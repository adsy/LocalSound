namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IDbTransactionRepository
    {
        Task CommitTransactionAsync();

        Task BeginTransactionAsync();
    }
}
