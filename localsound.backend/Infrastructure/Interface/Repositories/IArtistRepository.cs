﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IArtistRepository
    {
        Task<ServiceResponse> FollowArtistAsync(AppUser follower, string artistId);
        Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, UpdateArtistPersonalDetailsDto updateArtistDto);
        Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, UpdateArtistProfileDetailsDto updateArtistDto);
    }
}
