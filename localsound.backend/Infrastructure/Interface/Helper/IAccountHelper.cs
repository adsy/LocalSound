using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Infrastructure.Interface.Helper
{
    public interface IAccountHelper
    {
        IAppUserDto CreateArtistDto(Account artist);
        IAppUserDto CreateNonArtistDto(Account nonArtist);
    }
}
