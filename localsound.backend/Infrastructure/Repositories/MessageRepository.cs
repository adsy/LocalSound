using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(LocalSoundDbContext dbContext, ILogger<MessageRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> DismissMessageAsync(Guid userId, MessageEnum message)
        {
            try
            {
                var accountMessages = await _dbContext.AccountMessages.FirstOrDefaultAsync(x => x.AppUserId == userId);

                // Each account gets account messages added when created, so return error if it doesnt exist..
                if (accountMessages is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured while dismissing your message, please try again..."
                    };
                }

                switch (message) {
                    case MessageEnum.Onboarding:
                    {
                        accountMessages.CloseOnboardingMessage();
                        break;
                    }
                    default:break;
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var errorMessage = $"{nameof(MessageRepository)} - {nameof(DismissMessageAsync)} - {e.Message}";
                _logger.LogError(e, errorMessage);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while dismissing your message, please try again..."
                };
            }
        }
    }
}
