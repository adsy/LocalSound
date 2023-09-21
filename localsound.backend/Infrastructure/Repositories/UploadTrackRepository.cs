using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class UploadTrackRepository : IUploadTrackRepository
    {
        public readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<UploadTrackRepository> _logger;

        public UploadTrackRepository(LocalSoundDbContext dbContext, ILogger<UploadTrackRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        
    }
}
