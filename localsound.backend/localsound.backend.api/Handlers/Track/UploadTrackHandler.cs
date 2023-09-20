using localsound.backend.api.Commands.Track;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Track
{
    public class UploadTrackHandler : IRequestHandler<UploadTrackChunkCommand, ServiceResponse>,
        IRequestHandler<CompleteTrackUploadCommand, ServiceResponse>,
        IRequestHandler<TriggerTrackMergeCommand, ServiceResponse>
    {
        private readonly IUploadTrackService _uploadTrackService;

        public UploadTrackHandler(IUploadTrackService uploadTrackService)
        {
            _uploadTrackService = uploadTrackService;
        }

        public async Task<ServiceResponse> Handle(UploadTrackChunkCommand request, CancellationToken cancellationToken)
        {
            return await _uploadTrackService.UploadFileChunk(request.AppUserId, request.MemberId, request.PartialTrackId, request.File, request.ChunkId);
        }

        public async Task<ServiceResponse> Handle(CompleteTrackUploadCommand request, CancellationToken cancellationToken)
        {
            return await _uploadTrackService.CompleteTrackUpload(request.AppUserId, request.MemberId, request.PartialTrackId, request.FormData);
        }

        public async Task<ServiceResponse> Handle(TriggerTrackMergeCommand request, CancellationToken cancellationToken)
        {
            return await _uploadTrackService.MergeTrackChunks(request.PartialTrackId, request.TrackId);
        }
    }
}
