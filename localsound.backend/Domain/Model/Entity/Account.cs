using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Entity
{
    public class Account : CustomerType
    {
        [Key]
        [ForeignKey("User")]
        public Guid AppUserId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ProfileUrl { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? AboutSection { get; set; }
        public CustomerTypeEnum CustomerType { get; set; }
        public string MemberId { get; set; }

        public virtual ICollection<AccountGenre> Genres { get; set; }
        public virtual ICollection<ArtistEventType> EventTypes { get; set; }
        public virtual ICollection<ArtistEquipment> Equipment { get; set; }
        public virtual ICollection<ArtistPackage> Packages { get; set; }
        public virtual ICollection<ArtistBooking> Bookings { get; set; }
        public virtual ICollection<AccountImage> Images { get; set; }
        public virtual ICollection<ArtistFollower> Following { get; set; }
        public virtual ICollection<ArtistFollower> Followers { get; set; }
        public virtual ICollection<ArtistBooking> PartiesBooked { get; set; }
        public virtual ICollection<Notification> SentNotifications { get; set; }
        public virtual ICollection<Notification> ReceivedNotifications { get; set; }
        public virtual ICollection<SongLike> LikedSongs { get; set; }

        public virtual AccountMessages AccountMessages { get; set; }
        public virtual AppUser User { get; set; }

        public Account UpdateName(string name)
        {
            Name = name;
            return this;
        }

        public Account UpdateProfileUrl(string profileUrl)
        {
            ProfileUrl = profileUrl; 
            return this;
        }

        public Account UpdateAddress(string address)
        {
            Address = address; 
            return this;
        }

        public Account UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            return this;
        }

        public Account UpdateSocialLinks(string? soundcloud, string? spotify, string? youtube)
        {
            SoundcloudUrl = soundcloud;
            SpotifyUrl = spotify;
            YoutubeUrl = youtube;
            return this;
        }

        public Account UpdateAboutSection(string? aboutSection)
        {
            AboutSection = aboutSection;
            return this;
        }

        public Account UpdateGenres(ICollection<AccountGenre> genres)
        {
            Genres = genres;
            return this;
        }

        public Account UpdateEquipment(ICollection<ArtistEquipment> equipment)
        {
            Equipment = equipment;
            return this;
        }

        public Account UpdateEventTypes(ICollection<ArtistEventType> eventTypes)
        {
            EventTypes = eventTypes;
            return this;
        }
    }
}
