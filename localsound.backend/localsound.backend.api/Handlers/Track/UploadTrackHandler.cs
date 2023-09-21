using localsound.backend.api.Commands.Track;
using localsound.backend.api.Queries.Track;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Track
{
    public class UploadTrackHandler : IRequestHandler<GetUploadTrackDataQuery, ServiceResponse<TrackUploadSASDto>>,
        IRequestHandler<UploadTrackSupportingDetailsCommand, ServiceResponse>
    {
        private readonly IUploadTrackService _uploadTrackService;

        public UploadTrackHandler(IUploadTrackService uploadTrackService)
        {
            _uploadTrackService = uploadTrackService;
        }

        public async Task<ServiceResponse<TrackUploadSASDto>> Handle(GetUploadTrackDataQuery request, CancellationToken cancellationToken)
        {
            return await _uploadTrackService.GenerateTrackUploadSASDto(request.AppUserId, request.MemberId);
        }

        public async Task<ServiceResponse> Handle(UploadTrackSupportingDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _uploadTrackService.UploadTrackSupportingDetails(request.AppUserId, request.MemberId, request.TrackId, request.TrackData);
        }
    }
}