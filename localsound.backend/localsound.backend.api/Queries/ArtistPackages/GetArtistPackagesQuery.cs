using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.ArtistPackages
{
    public class GetArtistPackagesQuery : IRequest<ServiceResponse<List<ArtistPackageDto>>>
    {
        public string MemberId { get; set; }
    }
}
