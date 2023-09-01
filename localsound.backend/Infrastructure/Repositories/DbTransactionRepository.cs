using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace localsound.backend.Infrastructure.Repositories
{
    public class DbTransactionRepository : IDbTransactionRepository
    {
        private readonly LocalSoundDbContext _dbContext;

        private IDbContextTransaction _currentTransaction;

        public DbTransactionRepository(LocalSoundDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
