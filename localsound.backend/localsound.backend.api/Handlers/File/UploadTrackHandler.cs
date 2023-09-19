using localsound.backend.api.Commands.File;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.File
{
    public class UploadTrackHandler : IRequestHandler<UploadFileChunkCommand, ServiceResponse>
    {
        private readonly IUploadTrackService _fileService;

        public UploadTrackHandler(IUploadTrackService fileService)
        {
            _fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(UploadFileChunkCommand request, CancellationToken cancellationToken)
        {
            return await _fileService.UploadFileChunk(request.AppUserId, request.MemberId, request.PartialTrackId, request.File, request.ChunkId);
        }
    }
}
