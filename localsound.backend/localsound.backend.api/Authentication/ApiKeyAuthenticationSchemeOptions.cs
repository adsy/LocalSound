using Microsoft.AspNetCore.Authentication;

namespace localsound.backend.api.Authentication
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; }
    }
}
