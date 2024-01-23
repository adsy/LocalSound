using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Packages
{
    public class DeleteArtistPackageCommand : IRequest<ServiceResponse>
    {
        public string MemberId { get; set; }
        public Guid AppUserId { get; set; }
        public Guid PackageId { get; set; }
    }
}
