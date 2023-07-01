using AutoMapper;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;
using System.Security.Cryptography.X509Certificates;

namespace localsound.backend.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NonArtist, NonArtistDto>()
                .ForMember(dest => dest.Address, source => source.MapFrom(x => x.Address))
                .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.CustomerType, source => source.MapFrom(x => x.User.CustomerType));

            CreateMap<Artist, ArtistDto>()
                .ForMember(dest => dest.Address, source => source.MapFrom(x => x.Address))
                .ForMember(dest => dest.Name, source => source.MapFrom(x => x.Name))
                .ForMember(dest => dest.ProfileUrl, source => source.MapFrom(x => x.ProfileUrl))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.SoundcloudUrl, source => source.MapFrom(x => x.SoundcloudUrl))
                .ForMember(dest => dest.YoutubeUrl, source => source.MapFrom(x => x.YoutubeUrl))
                .ForMember(dest => dest.SpotifyUrl, source => source.MapFrom(x => x.SpotifyUrl))
                .ForMember(dest => dest.CustomerType, source => source.MapFrom(x => x.User.CustomerType));
        }
    }
}
