using AspNetCoreRateLimit;
using Azure.Storage.Blobs;
using localsound.backend.api.Extensions;
using localsound.backend.api.Middleware;
using localsound.backend.api.SignalR;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Infrastructure.Repositories;
using localsound.backend.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration().WriteTo.File(
                    path: "logs\\log-.txt",
                    outputTemplate: "{Timestamp:yyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<JwtSettingsAdaptor>(options => builder.Configuration.GetSection(JwtSettingsAdaptor.JwtSettings).Bind(options));
builder.Services.Configure<EmailSettingsAdaptor>(options => builder.Configuration.GetSection(EmailSettingsAdaptor.EmailSettingsKey).Bind(options));

var blobStorageSettings = new BlobStorageSettingsAdaptor();
builder.Configuration.GetSection(BlobStorageSettingsAdaptor.BlobSettings).Bind(blobStorageSettings);

var serviceBusSettings = new ServiceBusSettingsAdaptor();
builder.Configuration.GetSection(ServiceBusSettingsAdaptor.ServiceBusSettings).Bind(serviceBusSettings);

builder.Services.AddSingleton(new JwtSettingsAdaptor());
builder.Services.AddSingleton(new EmailSettingsAdaptor());
builder.Services.AddSingleton(blobStorageSettings);
builder.Services.AddSingleton(serviceBusSettings);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
await builder.Services.AddDbSeed(builder.Configuration);

builder.Services.AddSingleton<BlobServiceClient>(x =>
{
    var _blobServiceClient = new BlobServiceClient(blobStorageSettings.ConnectionString);
    var properties = _blobServiceClient.GetProperties();
    properties.Value.DefaultServiceVersion = "2013-08-15";
    _blobServiceClient.SetProperties(properties.Value);
    return _blobServiceClient;
});

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<IArtistRepository, ArtistRepository>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IEventTypeService, EventTypeService>();
builder.Services.AddTransient<IEventTypeRepository, EventTypeRepository>();
builder.Services.AddTransient<IDbTransactionRepository, DbTransactionRepository>();
builder.Services.AddTransient<IAccountImageRepository, AccountImageRepository>();
builder.Services.AddTransient<IAccountImageService, AccountImageService>();
builder.Services.AddTransient<IBlobRepository, BlobRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();
builder.Services.AddTransient<ITrackService,  TrackService>();
builder.Services.AddTransient<ITrackRepository,  TrackRepository>();
builder.Services.AddTransient<IServiceBusRepository, ServiceBusRepository>();
builder.Services.AddTransient<IPackageRepository, PackageRepository>();
builder.Services.AddTransient<IPackageService,  PackageService>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSAllowLocalHost5173",
      builder =>
      builder.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials() // <<< this is required for cookies to be set on the client - sets the 'Access-Control-Allow-Credentials' to true
        .WithExposedHeaders("www-authenticate")
     );
});

builder.Services.AddMemoryCache();

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.QuotaExceededMessage = "You're sending too many requests, please try again in 5 seconds...";

    options.EndpointWhitelist = new List<string>
    {
        "post:/api/token/refresh-token"
    };

    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "5s",
            Limit = 10,
        }
    };
});

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();


builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Localound.Backend v1"));
}

app.UseCors("CORSAllowLocalHost5173");

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notification");

app.Run();
