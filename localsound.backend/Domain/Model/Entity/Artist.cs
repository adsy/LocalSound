using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Domain.Model.Entity
{
    public class Artist : CustomerType
    {
        [Key]
        [ForeignKey("User")]
        public Guid AppUserId { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? AboutSection { get; set; }

        public virtual ICollection<ArtistGenre> Genres { get; set; }
        public virtual ICollection<ArtistEventType> EventTypes { get; set; }
        public virtual ICollection<ArtistEquipment> Equipment { get; set; }
        public virtual ICollection<ArtistFollower> Followers { get; set; }
        public virtual AppUser User { get; set; }

        public Artist UpdateName(string name)
        {
            Name = name;
            return this;
        }

        public Artist UpdateProfileUrl(string profileUrl)
        {
            ProfileUrl = profileUrl; 
            return this;
        }

        public Artist UpdateAddress(string address)
        {
            Address = address; 
            return this;
        }

        public Artist UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            return this;
        }

        public Artist UpdateSocialLinks(string? soundcloud, string? spotify, string? youtube)
        {
            SoundcloudUrl = soundcloud;
            SpotifyUrl = spotify;
            YoutubeUrl = youtube;
            return this;
        }

        public Artist UpdateAboutSection(string? aboutSection)
        {
            AboutSection = aboutSection;
            return this;
        }

        public Artist UpdateGenres(ICollection<ArtistGenre> genres)
        {
            Genres = genres;
            return this;
        }

        public Artist UpdateEquipment(ICollection<ArtistEquipment> equipment)
        {
            Equipment = equipment;
            return this;
        }

        public Artist UpdateEventTypes(ICollection<ArtistEventType> eventTypes)
        {
            EventTypes = eventTypes;
            return this;
        }
    }
}
