using FplDashboard.API.Factories;
using FplDashboard.API.Queries;
using FplDashboard.DataModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FplDashboardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FplDashboard")));
builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<DashboardQueries>();
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
