using FplDashboard.API.Features.Dashboard;
using FplDashboard.API.Features.Players;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.Features.Teams;
using FplDashboard.API.Infrastructure;
using FplDashboard.DataModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FplDashboardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FplDashboard")));
builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<DashboardQueries>();
builder.Services.AddScoped<TeamsQueries>();
builder.Services.AddScoped<PlayersQueries>();
builder.Services.AddMemoryCache();
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

var app = builder.Build();
app.UseCors("AllowAngularDev");

app.MapControllers();

app.Run();
