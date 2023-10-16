namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistTrackUploadDto
    {
        public Guid ArtistTrackUploadId { get; set; }
        public string TrackName { get; set; }
        public string TrackDescription { get; set; }
        public string TrackImageUrl { get; set; }
        public string TrackDataLocation { get; set; }
        public string TrackUrl { get; set; }
        public string WaveformUrl { get; set; }
        public double Duration { get; set; }
        public DateTime UploadDate { get; set; }

        public List<GenreDto> Genres { get; set; }
    }
}
