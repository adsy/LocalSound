using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace localsound.backend.api.Extensions
{
    public class ConfirmEmailTokenProviderExtension<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : class
    {
        private readonly TimeSpan _timeStep;

        public ConfirmEmailTokenProviderExtension()
        {
            _timeStep = TimeSpan.FromMinutes(1);
        }

        public override async Task<string> GenerateAsync(
          string purpose,
          UserManager<TUser> manager,
          TUser user)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            byte[] token = await manager.CreateSecurityTokenAsync(user);
            string async = CustomRfc6238AuthenticationService.GenerateCode
                (
                    token,
                    await this.GetUserModifierAsync(purpose, manager, user),
                    _timeStep
                )
                .ToString("D6", (IFormatProvider)CultureInfo.InvariantCulture);
            token = (byte[])null;
            return async;
        }

        public override async Task<bool> ValidateAsync(
          string purpose,
          string token,
          UserManager<TUser> manager,
          TUser user)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            int code;
            if (!int.TryParse(token, out code))
                return false;
            byte[] securityToken = await manager.CreateSecurityTokenAsync(user);
            string userModifierAsync = await this.GetUserModifierAsync(purpose, manager, user);
            return securityToken != null && CustomRfc6238AuthenticationService.ValidateCode(
                securityToken,
                code,
                userModifierAsync,
                _timeStep);
        }

        // It's just a dummy for inheritance.
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(true);
        }
    }
}
