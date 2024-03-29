﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IGenreService
    {
        Task<ServiceResponse<List<GenreDto>>> SearchGenreType(string name, CancellationToken cancellationToken);
    }
}
