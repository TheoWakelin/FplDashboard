using FplDashboard.API.Features.Dashboard;
using FplDashboard.API.Features.Players;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.Features.Teams;
using FplDashboard.API.Infrastructure;
using FplDashboard.DataModel;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();
        Configure(app);
        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<FplDashboardDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FplDashboard")));
        builder.Services.AddControllers();
        builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
        builder.Services.AddScoped<DashboardQueries>();
        builder.Services.AddScoped<TeamsQueries>();
        builder.Services.AddScoped<PlayersQueries>();
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICacheService, MemoryCacheService>();
        builder.Services.AddScoped<IGeneralQueries, GeneralQueries>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularDev",
                policy => policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
        });
    }

    private static void Configure(WebApplication app)
    {
        app.UseCors("AllowAngularDev");
        app.MapControllers();
    }
}