using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger, IAccountRepository accountRepository)
        {
            _messageRepository = messageRepository;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResponse> DismissMessageAsync(Guid userId, string memberId, MessageEnum message)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "An error occured while dismissing your message, please try again...");
                }

                var dismissResult = await _messageRepository.DismissMessageAsync(userId, message);

                if (!dismissResult.IsSuccessStatusCode)
                {
                    return dismissResult;
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var errorMessage = $"{nameof(MessageService)} - {nameof(DismissMessageAsync)} - {e.Message}";
                _logger.LogError(e, errorMessage);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while dismissing your message, please try again..."
                };
            }
        }
    }
}
