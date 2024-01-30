using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.ArtistPackages
{
    public class UpdateArtistPackageCommand : IRequest<ServiceResponse>
    {
        public Guid PackageId { get; set; }
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public ArtistPackageSubmissionDto PackageDto { get; set; }
    }
}
