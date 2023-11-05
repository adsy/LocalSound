﻿using localsound.backend.api.Commands.Artist;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Infrastructure.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Artist
{
    public class ArtistHandler : IRequestHandler<UpdateArtistPersonalDetailsCommand, ServiceResponse>,
        IRequestHandler<UpdateArtistProfileDetailsCommand, ServiceResponse>,
        IRequestHandler<FollowArtistCommand, ServiceResponse>
    {
        private readonly IArtistService _artistService;

        public ArtistHandler(IArtistService artistService)
        {
            _artistService = artistService;
        }

        public async Task<ServiceResponse> Handle(UpdateArtistPersonalDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _artistService.UpdateArtistPersonalDetails(request.UserId, request.MemberId, request.UpdateArtistDto);
        }

        public async Task<ServiceResponse> Handle(UpdateArtistProfileDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _artistService.UpdateArtistProfileDetails(request.UserId, request.MemberId, request.UpdateArtistDto);
        }

        public async Task<ServiceResponse> Handle(FollowArtistCommand request, CancellationToken cancellationToken)
        {
            return await _artistService.FollowArtist(request.UserId, request.MemberId, request.ArtistId);
        }
    }
}
