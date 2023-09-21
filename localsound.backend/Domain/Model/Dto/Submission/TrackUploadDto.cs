using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class TrackUploadDto
    {
        public string TrackName { get; set; }
        public string TrackFileExt { get; set; }
        public string TrackDescription { get; set; }
        public Guid GenreId { get; set; }
        public IFormFile TrackImage { get; set; }
        public string TrackImageExt { get; set; }
        public string FileLocation { get; set; }
    }
}