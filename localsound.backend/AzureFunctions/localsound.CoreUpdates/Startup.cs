using localsound.CoreUpdates;
using localsound.CoreUpdates.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace localsound.CoreUpdates
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddDbContext<LocalSoundDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("localSoundDb"));
            });
        }
    }
}
