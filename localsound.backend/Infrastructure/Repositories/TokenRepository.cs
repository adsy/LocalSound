using AutoMapper;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace localsound.backend.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettingsAdaptor _settings;
        private readonly ILogger<TokenRepository> _logger;
        private readonly LocalSoundDbContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public TokenRepository(UserManager<AppUser> userManager,
            IOptions<JwtSettingsAdaptor> settings,
            ILogger<TokenRepository> logger,
            LocalSoundDbContext context,
            IAccountRepository accountRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _settings = settings.Value;
            _logger = logger;
            _context = context;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        // Create new Access Token
        public string CreateToken(List<Claim> claims)
        {
            // create key and use it to create credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // create securityTokenDescriptor with the claims and credentials + expiry date
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = credentials
            };

            // create token handler which returns token and writes it as response

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public List<Claim> GetClaims(AppUser user)
        {
            // create claims for user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            if (user.CustomerType == CustomerType.Artist)
            {
                claims.Add(new Claim(ClaimTypes.Role, CustomerType.Artist.ToString()));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, CustomerType.Artist.ToString()));
            }

            return claims;
        }
    }
}
