using FplDashboard.DataModel;
using FplDashboard.ETL;
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
    .AddScoped<TeamGameWeekSyncService>()
    .AddScoped<PlayerGameWeekSyncService>()
    .AddScoped<PlayerNewsSyncService>()
    .AddHostedService<FplSyncService>();

var host = builder.Build();
host.Run();
