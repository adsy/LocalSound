using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class ArtistPackageSubmissionDto
    {
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public string PackagePrice { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public string? DeletedPhotoIds { get; set; }
        public string PackageEquipment { get; set; }
    }
}
