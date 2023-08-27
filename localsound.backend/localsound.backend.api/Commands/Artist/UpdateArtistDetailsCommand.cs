using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Artist
{
    public class UpdateArtistDetailsCommand : IRequest<ServiceResponse>
    {
        public UpdateArtistDetailsCommand(UpdateArtistDto updateArtistDto)
        {
            UpdateArtistDto = updateArtistDto;
        }
        public UpdateArtistDto UpdateArtistDto { get; set; }

    }
}
