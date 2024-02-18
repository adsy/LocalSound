using System.Text.Json.Serialization;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistTrackUploadDto
    {
        public int ArtistTrackUploadId { get; set; }
        public string TrackName { get; set; }
        public string TrackDescription { get; set; }
        public string TrackImageUrl { get; set; }
        public string ArtistProfile { get; set; }
        public string ArtistName { get; set; }
        public string ArtistMemberId { get; set; }
        public string TrackUrl { get; set; }
        public double Duration { get; set; }
        public DateTime UploadDate { get; set; }

        public bool SongLiked { get; set; }
        public int LikeCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SongLikeId { get;set; }

        public List<GenreDto> Genres { get; set; }
    }
}
