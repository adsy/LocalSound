﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<NonArtist>> AddNonArtistToDbAsync(NonArtist nonArtist);
        Task<ServiceResponse<Artist>> AddArtistToDbAsync(Artist artist);
        Task<ServiceResponse<Artist>> GetArtistFromDbAsync(Guid id);
        Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(Guid id);
    }
}
