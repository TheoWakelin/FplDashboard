using FplDashboard.DataModel;
using FplDashboard.ETL;
using FplDashboard.ETL.Services;
using Microsoft.EntityFrameworkCore;
using FplSyncRunner = FplDashboard.ETL.Services.FplSyncRunner;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services
    .AddDbContext<FplDashboardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FplDashboard")))
    .AddHttpClient()
    .AddScoped<TeamSyncService>()
    .AddScoped<GameWeekSyncService>()
    .AddScoped<FplSyncRunner>()
    .AddScoped<FplSyncRunnerTestsTeamGameWeekSyncService>()
    .AddScoped<FplSyncRunnerTestsPlayerGameWeekSyncService>()
    .AddScoped<FplSyncRunnerTestsPlayerNewsSyncService>()
    .AddHostedService<FplSyncHost>();

var host = builder.Build();
host.Run();
