using Azure.Messaging.ServiceBus;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace localsound.backend.Infrastructure.Repositories
{
    public class ServiceBusRepository : IServiceBusRepository
    {
        private readonly ILogger<ServiceBusRepository> _logger;
        private readonly ServiceBusSettingsAdaptor _options;

        private ServiceBusClient _deleteQueueClient;
        private ServiceBusSender _deleteQueueSender;

        public ServiceBusRepository(ILogger<ServiceBusRepository> logger, ServiceBusSettingsAdaptor options)
        {
            _logger = logger;
            _options = options;


            // deleteEntityQueue
            _deleteQueueClient = new ServiceBusClient(_options.DeleteEntityQueue.ConnectionString);
            _deleteQueueSender = _deleteQueueClient.CreateSender(_options.DeleteEntityQueue.QueueName);
        }

        public async Task<ServiceResponse> SendDeleteQueueEntry<T>(T message)
        {
            try
            {
                var content = JsonSerializer.Serialize(message, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });

                await _deleteQueueSender.SendMessageAsync(new ServiceBusMessage(content));

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var errorMessage = $"{nameof(ServiceBusRepository)} - {nameof(SendDeleteQueueEntry)} - {e.Message}";
                _logger.LogError(e, errorMessage);
                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
