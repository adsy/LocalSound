using AutoMapper;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Helper;

namespace localsound.backend.Infrastructure.Helper
{
    public class AccountHelper : IAccountHelper
    {
        private readonly IMapper _mapper;

        public AccountHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IAppUserDto CreateArtistDto(Account artist)
        {
            var returnDto = _mapper.Map<ArtistDto>(artist);
            returnDto.Images = _mapper.Map<List<AccountImageDto>>(artist.Images);

            if (artist.AccountMessages != null)
            {
                if (!artist.AccountMessages.OnboardingMessageClosed)
                {
                    returnDto.Messages.Add("onboardingMessageClosed", false);
                }
            }

            if (artist.Followers != null)
            {
                returnDto.FollowerCount = artist.Followers.Count;
            }
            else
            {
                returnDto.FollowerCount = 0;
            }

            if (artist.Following != null)
            {
                returnDto.FollowingCount = artist.Following.Count;
            }
            else
            {
                returnDto.FollowingCount = 0;
            }

            returnDto.CanAddPackage = artist.Packages?.Count < 3;


            return returnDto;
        }

        public IAppUserDto CreateNonArtistDto(Account nonArtist)
        {
            var returnDto = _mapper.Map<NonArtistDto>(nonArtist);
            returnDto.Images = _mapper.Map<List<AccountImageDto>>(nonArtist.Images);

            if (nonArtist.AccountMessages != null)
            {
                if (!nonArtist.AccountMessages.OnboardingMessageClosed)
                {
                    returnDto.Messages.Add("onboardingMessageClosed", false);
                }
            }

            if (nonArtist.Following != null)
            {
                returnDto.FollowingCount = nonArtist.Following.Count;
            }
            else
            {
                returnDto.FollowingCount = 0;
            }

            return returnDto;
        }
    }
}
