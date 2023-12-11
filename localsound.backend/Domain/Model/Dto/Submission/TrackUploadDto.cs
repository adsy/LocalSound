using localsound.backend.Domain.Model.Dto.Entity;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class TrackUploadDto
    {
        public string TrackName { get; set; }
        public string TrackFileExt { get; set; }
        public string TrackDescription { get; set; }
        public List<GenreDto> Genres { get; set; }
        public IFormFile? TrackImage { get; set; }
        public string? TrackImageExt { get; set; }
        public string FileLocation { get; set; }
        public string TrackUrl { get; set; }
        public string WaveformUrl { get; set; }
        public string Duration { get; set; }
        public string FileSize { get; set; }
    }
}