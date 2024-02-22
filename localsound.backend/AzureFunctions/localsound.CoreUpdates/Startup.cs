using Azure.Storage.Blobs;
using localsound.CoreUpdates;
using localsound.CoreUpdates.Persistence;
using localsound.CoreUpdates.Repository;
using localsound.CoreUpdates.Services;
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

            builder.Services.AddTransient<IAccountImageService, AccountImageService>();
            builder.Services.AddTransient<IPackageService, PackageService>();
            builder.Services.AddTransient<ITrackService, TrackService>();
            builder.Services.AddTransient<IBlobRepository, BlobRepository>();
            builder.Services.AddTransient<IDbOperationRepository, DbOperationRepository>();

            builder.Services.AddSingleton<BlobServiceClient>(x =>
            {
                var _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("blobStorageConnectionString"));
                var properties = _blobServiceClient.GetProperties();
                properties.Value.DefaultServiceVersion = "2013-08-15";
                _blobServiceClient.SetProperties(properties.Value);
                return _blobServiceClient;
            });

            builder.Services.AddDbContext<LocalSoundDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("localSoundDb"));
            });
        }
    }
}
