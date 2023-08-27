using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Artist
{
    public class UpdateArtistDetailsCommand : IRequest<ServiceResponse>
    {
        public UpdateArtistDto UpdateArtistDto { get; set; }

    }
}
