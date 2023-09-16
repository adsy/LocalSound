﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Artist
{
    public class UpdateArtistPersonalDetailsCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public UpdateArtistPersonalDetailsDto UpdateArtistDto { get; set; }

    }
}
