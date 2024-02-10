using AutoMapper;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, NonArtistDto>()
                .ForMember(dest => dest.Address, source => source.MapFrom(x => x.Address))
                .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.CustomerType, source => source.MapFrom(x => x.CustomerType))
                .ForMember(dest => dest.EmailConfirmed, source => source.MapFrom(x => x.User.EmailConfirmed))
                .ForMember(dest => dest.ProfileUrl, source => source.MapFrom(x => x.ProfileUrl))
                .ForMember(dest => dest.AccountSetupCompleted, source => source.MapFrom(x => x.AccountOnboarding.AccountSetupCompleted))
                .ForMember(dest => dest.MemberId, source => source.MapFrom(x => x.MemberId));

            CreateMap<Account, ArtistDto>()
                .ForMember(dest => dest.Address, source => source.MapFrom(x => x.Address))
                .ForMember(dest => dest.Name, source => source.MapFrom(x => x.Name))
                .ForMember(dest => dest.ProfileUrl, source => source.MapFrom(x => x.ProfileUrl))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.SoundcloudUrl, source => source.MapFrom(x => x.SoundcloudUrl))
                .ForMember(dest => dest.YoutubeUrl, source => source.MapFrom(x => x.YoutubeUrl))
                .ForMember(dest => dest.SpotifyUrl, source => source.MapFrom(x => x.SpotifyUrl))
                .ForMember(dest => dest.CustomerType, source => source.MapFrom(x => x.CustomerType))
                .ForMember(dest => dest.AboutSection, source => source.MapFrom(x => x.AboutSection))
                .ForMember(dest => dest.EmailConfirmed, source => source.MapFrom(x => x.User.EmailConfirmed))
                .ForMember(dest => dest.FollowerCount, source => source.Ignore())
                .ForMember(dest => dest.FollowingCount, source => source.Ignore())
                .ForMember(dest => dest.AccountSetupCompleted, source => source.MapFrom(x => x.AccountOnboarding.AccountSetupCompleted))
                .ForMember(dest => dest.MemberId, source => source.MapFrom(x => x.MemberId));

            CreateMap<Genre, GenreDto>()
                .ForMember(dest => dest.GenreId, source => source.MapFrom(x => x.GenreId))
                .ForMember(dest => dest.GenreName, source => source.MapFrom(x => x.GenreName));

            CreateMap<AccountGenre, GenreDto>()
                .ForMember(dest => dest.GenreId, source => source.MapFrom(x => x.Genre.GenreId))
                .ForMember(dest => dest.GenreName, source => source.MapFrom(x => x.Genre.GenreName));

            CreateMap<EventType, EventTypeDto>()
                .ForMember(dest => dest.EventTypeId, source => source.MapFrom(x => x.EventTypeId))
                .ForMember(dest => dest.EventTypeName, source => source.MapFrom(x => x.EventTypeName));

            CreateMap<ArtistEventType, EventTypeDto>()
                .ForMember(dest => dest.EventTypeId, source => source.MapFrom(x => x.EventType.EventTypeId))
                .ForMember(dest => dest.EventTypeName, source => source.MapFrom(x => x.EventType.EventTypeName));

            CreateMap<ArtistEquipment, EquipmentDto>()
                .ForMember(dest => dest.EquipmentId, source => source.MapFrom(x => x.EquipmentId))
                .ForMember(dest => dest.EquipmentName, source => source.MapFrom(x => x.EquipmentName));

            CreateMap<AccountImage, AccountImageDto>()
                .ForMember(dest => dest.AccountImageTypeId, source => source.MapFrom(x => x.AccountImageTypeId))
                .ForMember(dest => dest.AccountImageUrl, source => source.MapFrom(x => x.AccountImageUrl));

            CreateMap<ArtistTrackUpload, ArtistTrackUploadDto>()
                .ForMember(dest => dest.ArtistTrackUploadId, source => source.MapFrom(x => x.ArtistTrackUploadId))
                .ForMember(dest => dest.TrackName, source => source.MapFrom(x => x.TrackName))
                .ForMember(dest => dest.TrackDescription, source => source.MapFrom(x => x.TrackDescription))
                .ForMember(dest => dest.TrackImageUrl, source => source.MapFrom(x => x.TrackImageUrl))
                .ForMember(dest => dest.TrackDataLocation, source => source.MapFrom(x => x.TrackData.FileLocation))
                .ForMember(dest => dest.TrackUrl, source => source.MapFrom(x => x.TrackUrl))
                .ForMember(dest => dest.WaveformUrl, source => source.MapFrom(x => x.WaveformUrl))
                .ForMember(dest => dest.Duration, source => source.MapFrom(x => x.Duration))
                .ForMember(dest => dest.UploadDate, source => source.MapFrom(x => x.UploadDate))
                .ForMember(dest => dest.ArtistName, source => source.MapFrom(x => x.Artist.Name))
                .ForMember(dest => dest.ArtistMemberId, source => source.MapFrom(x => x.Artist.MemberId))
                .ForMember(dest => dest.ArtistProfile, source => source.MapFrom(x => x.Artist.ProfileUrl));

            CreateMap<ArtistTrackGenre, GenreDto>()
                .ForMember(dest => dest.GenreId, source => source.MapFrom(x => x.Genre.GenreId))
                .ForMember(dest => dest.GenreName, source => source.MapFrom(x => x.Genre.GenreName));
        }
    }
}
