using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Packages
{
    public class CreateArtistPackageCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public ArtistPackageSubmissionDto PackageDto { get; set; }
    }
}
