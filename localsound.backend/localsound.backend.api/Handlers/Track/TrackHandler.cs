using localsound.backend.api.Commands.Track;
using localsound.backend.api.Queries.Track;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Track
{
    public class TrackHandler : IRequestHandler<GetUploadTrackDataQuery, ServiceResponse<TrackUploadSASDto>>,
        IRequestHandler<AddTrackSupportingDetailsCommand, ServiceResponse>,
        IRequestHandler<GetArtistTracksQuery, ServiceResponse<TrackListResponseDto>>,
        IRequestHandler<GetArtistTrackQuery, ServiceResponse<ArtistTrackUploadDto>>,
        IRequestHandler<UpdateTrackSupportingDetailsCommand, ServiceResponse>
    {
        private readonly ITrackService _trackService;

        public TrackHandler(ITrackService trackService)
        {
            _trackService = trackService;
        }

        public async Task<ServiceResponse<TrackUploadSASDto>> Handle(GetUploadTrackDataQuery request, CancellationToken cancellationToken)
        {
            return await _trackService.GenerateTrackUploadSASDto(request.AppUserId, request.MemberId);
        }

        public async Task<ServiceResponse> Handle(AddTrackSupportingDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.UploadTrackSupportingDetails(request.AppUserId, request.MemberId, request.TrackId, request.TrackData);
        }

        public async Task<ServiceResponse<TrackListResponseDto>> Handle(GetArtistTracksQuery request, CancellationToken cancellationToken)
        {
            return await _trackService.GetArtistTracks(request.MemberId, request.Page);
        }

        public async Task<ServiceResponse<ArtistTrackUploadDto>> Handle(GetArtistTrackQuery request, CancellationToken cancellationToken)
        {
            return await _trackService.GetArtistTrack(request.MemberId, request.TrackId);
        }

        public async Task<ServiceResponse> Handle(UpdateTrackSupportingDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.UpdateTrackSupportingDetails(request.AppUserId, request.MemberId, request.TrackId, request.TrackData); 
        }
    }
}