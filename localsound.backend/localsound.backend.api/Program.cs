using localsound.backend.api.Extensions;
using localsound.backend.api.Middleware;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Infrastructure.Repositories;
using localsound.backend.Infrastructure.Services;
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


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
await builder.Services.AddDbSeed(builder.Configuration);


builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

builder.Services.AddTransient<ITokenRepository, TokenRepository>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Localound.Backend v1"));
}

app.UseHttpsRedirection();

app.UseCors("CORSAllowLocalHost5173");

app.UseAuthorization();

app.MapControllers();

app.Run();
