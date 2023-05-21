using Microsoft.AspNetCore.Identity;

namespace localsound.backend.api.Extensions
{
    public class RefreshTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public RefreshTokenProviderOptions()
        {
            Name = "RefreshTokenProviderExtension";
            TokenLifespan = TimeSpan.FromDays(7);
        }
    }
}
