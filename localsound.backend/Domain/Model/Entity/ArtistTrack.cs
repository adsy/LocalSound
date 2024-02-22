using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrack
    {
        public int ArtistTrackId { get; set; }
        [ForeignKey(nameof(Artist))]
        public Guid AppUserId { get; set; }
        public string ArtistMemberId { get; set; }
        public string TrackName { get; set; }
        public string TrackDescription { get; set; }
        public string TrackUrl { get; set; }
        public double Duration { get; set; }
        public DateTime UploadDate { get; set; }
        public int FileSizeInBytes { get; set; }
        public int LikeCount { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual Account Artist {get;set;}
        public virtual ICollection<ArtistTrackGenre>  Genres { get; set;}
        public virtual ArtistTrackAudioFileContent TrackData { get; set; }
        public virtual ICollection<ArtistTrackImage> TrackImage { get; set; }

        public virtual List<SongLike> SongLikes { get; set; }

        public ArtistTrack UpdateDetails(string name, string description)
        {
            TrackName = name;
            TrackDescription = description;
            return this;
        }

        public ArtistTrack UpdateGenres(ICollection<ArtistTrackGenre> genres)
        {
            Genres = genres;
            return this;
        }

        public ArtistTrack UpdateExistingImageToDeleted()
        {
            var existingImage = TrackImage?.FirstOrDefault(x => !x.ToBeDeleted);
            if (TrackImage != null && existingImage != null)
            {
                existingImage.ToBeDeleted = true;
            }

            return this;
        }
    }
}
