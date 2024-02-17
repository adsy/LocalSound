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
        IRequestHandler<GetTracksQuery, ServiceResponse<TrackListResponseDto>>,
        IRequestHandler<GetArtistTrackQuery, ServiceResponse<ArtistTrackUploadDto>>,
        IRequestHandler<UpdateTrackSupportingDetailsCommand, ServiceResponse>,
        IRequestHandler<DeleteArtistTrackCommand, ServiceResponse>,
        IRequestHandler<LikeArtistTrackCommand, ServiceResponse>,
        IRequestHandler<UnlikeArtistTrackCommand, ServiceResponse>
    {
        private readonly ITrackService _trackService;

        public TrackHandler(ITrackService trackService)
        {
            _trackService = trackService;
        }

        public async Task<ServiceResponse<TrackUploadSASDto>> Handle(GetUploadTrackDataQuery request, CancellationToken cancellationToken)
        {
            return await _trackService.GenerateTrackUploadSASDtoAsync(request.AppUserId, request.MemberId);
        }

        public async Task<ServiceResponse> Handle(AddTrackSupportingDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.UploadTrackSupportingDetailsAsync(request.UserId, request.MemberId, request.TrackId, request.TrackData);
        }

        public async Task<ServiceResponse<ArtistTrackUploadDto>> Handle(GetArtistTrackQuery request, CancellationToken cancellationToken)
        {
            return await _trackService.GetArtistTrackAsync(request.MemberId, request.TrackId);
        }

        public async Task<ServiceResponse> Handle(UpdateTrackSupportingDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.UpdateTrackSupportingDetailsAsync(request.UserId, request.MemberId, request.TrackId, request.TrackData); 
        }

        public async Task<ServiceResponse> Handle(DeleteArtistTrackCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.DeleteArtistTrackAsync(request.UserId, request.MemberId, request.TrackId);
        }

        public async Task<ServiceResponse> Handle(LikeArtistTrackCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.LikeArtistTrackAsync(request.TrackId, request.ArtistMemberId, request.UserId, request.MemberId);
        }

        public async Task<ServiceResponse> Handle(UnlikeArtistTrackCommand request, CancellationToken cancellationToken)
        {
            return await _trackService.UnikeArtistTrackAsync(request.TrackId, request.ArtistMemberId, request.UserId, request.MemberId);
        }

        public async Task<ServiceResponse<TrackListResponseDto>> Handle(GetTracksQuery request, CancellationToken cancellationToken)
        {
            return await _trackService.GetTracksByPlaylistTypeAsync(request.UserId, request.MemberId, request.LastUploadDate, request.PlaylistType);
        }
    }
}