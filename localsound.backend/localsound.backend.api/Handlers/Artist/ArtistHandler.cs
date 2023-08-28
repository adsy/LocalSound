using localsound.backend.api.Commands.Artist;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Persistence.DbContext;
using MediatR;

namespace localsound.backend.api.Handlers.Artist
{
    public class ArtistHandler : IRequestHandler<UpdateArtistDetailsCommand, ServiceResponse>
    {
        private readonly IArtistService _artistService;

        public ArtistHandler(IArtistService artistService)
        {
            _artistService = artistService;
        }

        public async Task<ServiceResponse> Handle(UpdateArtistDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _artistService.UpdateArtistDetails(request.UserId, request.MemberId, request.UpdateArtistDto);
        }
    }
}
