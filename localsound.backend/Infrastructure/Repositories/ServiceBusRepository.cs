using Azure.Messaging.ServiceBus;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class ServiceBusRepository : IServiceBusRepository
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<ServiceBusRepository> _logger;

        public ServiceBusRepository(ServiceBusClient serviceBusClient, ILogger<ServiceBusRepository> logger)
        {
            _serviceBusClient = serviceBusClient;
            _logger = logger;
        }

        public async Task<ServiceResponse> PushMessageToQueueAsync(string queueName, string message)
        {
            try
            {
                var sender = _serviceBusClient.CreateSender(queueName);

                await sender.SendMessageAsync(new ServiceBusMessage(message));

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var errorMessage = $"{nameof(ServiceBusRepository)} - {nameof(PushMessageToQueueAsync)} - {e.Message}";
                _logger.LogError(e, errorMessage);
                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
