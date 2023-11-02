using localsound.backend.Domain.Model.Dto.Entity;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class TrackUpdateDto
    {
        public string TrackName { get; set; }
        public string TrackDescription { get; set; }
        public List<GenreDto> Genres { get; set; }
        public IFormFile? TrackImage { get; set; }
        public string? TrackImageExt { get; set; }
    }
}
