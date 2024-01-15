using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using localsound.CoreUpdates.Dto;
using localsound.CoreUpdates.Dto.Enum;
using localsound.CoreUpdates.Dto.QueueMessage;
using localsound.CoreUpdates.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace localsound.CoreUpdates
{
    public class DeleteEntityFromAzureStorage
    {
        private readonly IAccountImageService _accountImageService;

        public DeleteEntityFromAzureStorage(IAccountImageService accountImageService)
        {
            _accountImageService = accountImageService;
        }

        [FunctionName("DeleteEntityFromAzureStorage")]
        public async Task Run(
           [ServiceBusTrigger("queue", Connection = "ServiceBusConnection")]
           ServiceBusReceivedMessage message,
           ServiceBusMessageActions messageActions)
        {
            var queueMessage = JsonSerializer.Deserialize<QueueMessageDto>(message.Body.ToString());

            switch(queueMessage.Command) 
            {
                case (DeleteEntityTypeEnum.DeleteAccountImage):
                    {
                        DeleteAccountImageDto dto = JsonSerializer.Deserialize<DeleteAccountImageDto>(queueMessage.Data.ToString());
                        var isDeleteOpSuccess = await _accountImageService.DeleteAccountImage(dto.UserId, dto.AccountImageId, dto.UploadLocation);

                        if (isDeleteOpSuccess)
                        {
                            await messageActions.CompleteMessageAsync(message);
                        }
                        else
                        {
                            await messageActions.AbandonMessageAsync(message);
                        }
                        break;
                    }
            }
        }
    }
}
