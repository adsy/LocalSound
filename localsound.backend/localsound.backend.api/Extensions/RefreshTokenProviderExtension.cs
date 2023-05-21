using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace localsound.backend.api.Extensions
{
    public class RefreshTokenProviderExtension<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public RefreshTokenProviderExtension(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
