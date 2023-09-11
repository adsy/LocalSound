using Microsoft.AspNetCore.Identity;

namespace localsound.backend.api.Extensions
{
    public class ConfirmEmailTokenProviderOptions: DataProtectionTokenProviderOptions
    {
        public ConfirmEmailTokenProviderOptions()
        {
            Name = "ConfirmEmailTokenProviderExtension";
            TokenLifespan = TimeSpan.FromMinutes(2);
        }
    }
}
