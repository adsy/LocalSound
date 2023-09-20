﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.ServiceBus;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace localsound.backend.Infrastructure.Services
{
    public class UploadTrackService : IUploadTrackService
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IServiceBusRepository _serviceBusRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUploadTrackRepository _uploadTrackRepository;
        private readonly IDbTransactionRepository _dbTransactionRepository;
        private readonly ILogger<UploadTrackService> _logger;
        private readonly ServiceBusSettingsAdaptor _serviceBusSettings;

        public UploadTrackService(IBlobRepository blobRepository, ILogger<UploadTrackService> logger, IAccountRepository accountRepository, IUploadTrackRepository uploadTrackRepository, IDbTransactionRepository dbTransactionRepository, IServiceBusRepository serviceBusRepository, ServiceBusSettingsAdaptor serviceBusSettings)
        {
            _blobRepository = blobRepository;
            _logger = logger;
            _accountRepository = accountRepository;
            _uploadTrackRepository = uploadTrackRepository;
            _dbTransactionRepository = dbTransactionRepository;
            _serviceBusRepository = serviceBusRepository;
            _serviceBusSettings = serviceBusSettings;
        }

        public async Task<ServiceResponse> CompleteTrackUpload(Guid userId, string memberId, Guid partialTrackId)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "There was an error while updating your details, please try again.");
                }

                await _dbTransactionRepository.BeginTransactionAsync();

                //TODO: Get full track details from client and save in DB

                var trackName = "test-track";
                var fileExt = ".mp3";
                var fileLocation = $"[tracks]/account/{userId}/uploads/{trackName}{fileExt}";

                var result = await _uploadTrackRepository.AddArtistTrackToDbAsync(userId, trackName, fileLocation, fileExt);

                if (!result.IsSuccessStatusCode || result.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
                }
                var message = new CompleteTrackMessageDto
                {
                    ArtistTrackUploadId = result.ReturnData.ArtistTrackUploadId,
                    PartialTrackId = partialTrackId
                };

                var serviceBusResult = await _serviceBusRepository.PushMessageToQueueAsync(_serviceBusSettings.Queues.TrackComplete, JsonSerializer.Serialize(message));

                if (!serviceBusResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
                }

                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackService)} - {nameof(CompleteTrackUpload)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
            }
        }

        public async Task<ServiceResponse> UploadFileChunk(Guid userId, string memberId, Guid partialTrackId, IFormFile file, int chunkId)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "There was an error while updating your details, please try again.");
                }

                await _dbTransactionRepository.BeginTransactionAsync();

                var fileLocation = $"[tracks]/account/{userId}/chunks/partialTrackId-{partialTrackId}";

                var addToDbResult = await _uploadTrackRepository.UploadTrackChunkAsync(fileLocation, partialTrackId, userId, chunkId);

                if (!addToDbResult.IsSuccessStatusCode || addToDbResult.ReturnData == null)
                {
                    // Log error here?
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                var uploadResult = await _blobRepository.UploadBlobAsync(addToDbResult.ReturnData.FileContent.FileLocation, file);

                if (!uploadResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(uploadResult.StatusCode);
                }

                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(UploadTrackService)} - {nameof(UploadFileChunk)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
            }
        }
    }
}
