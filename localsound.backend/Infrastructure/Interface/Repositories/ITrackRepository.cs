﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITrackRepository
    {
        Task<ServiceResponse<int>> AddArtistTrackUploadAsync(ArtistTrackUpload track);
        Task<ServiceResponse> DeleteTrackAsync(ArtistTrackUpload track);
        Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(string memberId, int trackId);
        Task<ServiceResponse<List<ArtistTrackUpload>>> GetArtistTracksAsync(string memberId, int? lastTrackId);
        Task<ServiceResponse> LikeArtistTrackAsync(string memberId, string artistMemberId, int trackId);
        Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, string artistMemberId, int trackId);
        Task<ServiceResponse> UpdateArtistTrackUploadAsync(Account account, int trackId, string trackName, string trackDescription, List<GenreDto> genres, string? trackImageExt, FileContent? newTrackImage, string newTrackImageUrl);
        Task<ServiceResponse<List<int>>> GetLikedSongsIdsAsync(string memberId);
        Task<ServiceResponse<List<ArtistTrackUpload>>> GetLikedSongsAsync(string memberId, int? lastTrackId);
    }
}
