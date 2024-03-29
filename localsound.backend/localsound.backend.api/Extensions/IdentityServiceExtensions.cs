﻿using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Persistence.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace localsound.backend.api.Extensions
{
    public static class IdentityServiceExtensions
    {
        private const string Key = "ApiKey";

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.AuthenticatorTokenProvider = "RefreshTokenProviderExtension";
                options.Tokens.EmailConfirmationTokenProvider = "ConfirmEmailTokenProviderExtension";
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                .AddTokenProvider<RefreshTokenProviderExtension<AppUser>>("RefreshTokenProviderExtension")
                .AddTokenProvider<ConfirmEmailTokenProviderExtension<AppUser>>("ConfirmEmailTokenProviderExtension")
                .AddEntityFrameworkStores<LocalSoundDbContext>()
                .AddSignInManager<SignInManager<AppUser>>();

            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromMinutes(30));

            var jwtSettings = services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettingsAdaptor>>();
            services.Configure<RefreshTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(7));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false, // set to true when deployed
                        ValidateAudience = false, // set to true when deployed
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateLifetime = true
                    };
                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                            return Task.CompletedTask;
                        }
                    };
                });
            
            return services;
        }
    }
}
