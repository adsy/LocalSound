using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using localsound.CoreUpdates.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using LocalSound.Shared.Package.ServiceBus.Dto;
using LocalSound.Shared.Package.ServiceBus.Dto.QueueMessage;
using LocalSound.Shared.Package.ServiceBus.Dto.Enum;

namespace localsound.CoreUpdates
{
    public class DeleteEntityFromAzureStorage
    {
        private readonly IAccountImageService _accountImageService;
        private readonly IPackageService _packageService;
        private readonly ITrackService _trackService;

        public DeleteEntityFromAzureStorage(IAccountImageService accountImageService, IPackageService packageService, ITrackService trackService)
        {
            _accountImageService = accountImageService;
            _packageService = packageService;
            _trackService = trackService;
        }

        [FunctionName("DeleteEntityFromAzureStorage")]
        public async Task Run(
           [ServiceBusTrigger("delete-entity-queue", Connection = "deleteEntityQueue")]
           ServiceBusReceivedMessage message,
           ServiceBusMessageActions messageActions)
        {
            var queueMessage = JsonSerializer.Deserialize<QueueMessageDto>(message.Body.ToString());

            switch(queueMessage.Command) 
            {
                case (DeleteEntityTypeEnum.DeleteAccountImage):
                    {
                        DeleteAccountImageDto dto = JsonSerializer.Deserialize<DeleteAccountImageDto>(queueMessage.Data.ToString());
                        var isDeleteOpSuccess = await _accountImageService.DeleteAccountImage(dto.UserId, dto.AccountImageId);

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
                case (DeleteEntityTypeEnum.DeletePackagePhotos):
                    {
                        DeletePackagePhotosDto dto = JsonSerializer.Deserialize<DeletePackagePhotosDto>(queueMessage.Data.ToString());
                        var isDeleteOpSuccess = await _packageService.DeletePackagePhotos(dto.UserId, dto.PackageId);

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
                case (DeleteEntityTypeEnum.DeleteArtistTrack):
                    {
                        DeleteArtistTrackDto dto = JsonSerializer.Deserialize<DeleteArtistTrackDto>(queueMessage.Data.ToString());
                        var isDeleteOpSuccess = await _trackService.DeleteArtistTrack(dto.ArtistTrackId, dto.ArtistMemberId);

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
                case (DeleteEntityTypeEnum.DeleteArtistTrackImage):
                    {
                        DeleteArtistTrackDto dto = JsonSerializer.Deserialize<DeleteArtistTrackDto>(queueMessage.Data.ToString());
                        var isDeleteOpSuccess = await _trackService.DeleteArtistTrackImage(dto.ArtistTrackId, dto.ArtistMemberId);

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
