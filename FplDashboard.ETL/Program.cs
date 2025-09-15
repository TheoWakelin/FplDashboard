using FplDashboard.DataModel;
using FplDashboard.ETL;
using FplDashboard.ETL.Interfaces;
using FplDashboard.ETL.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services
    .AddDbContext<FplDashboardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FplDashboard")))
    .AddHttpClient()
    .AddScoped<TeamSyncService>()
    .AddScoped<GameWeekSyncService>()
    .AddScoped<PlayerSyncService>()
    .AddScoped<FplSyncRunner>()
    .AddScoped<TeamGameWeekSyncService>()
    .AddScoped<PlayerGameWeekSyncService>()
    .AddScoped<PlayerNewsSyncService>()
    .AddScoped<FixtureSyncService>()
    .AddScoped<IFplApiClient, FplApiClient>()
    .AddHostedService<FplSyncHost>();

var host = builder.Build();
host.Run();
