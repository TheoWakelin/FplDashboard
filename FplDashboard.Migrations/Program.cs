using FplDashboard.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FplDashboard.Migrations;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Applying migrations...");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.AddDbContext<FplDashboardDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("FplDashboard"),
                sql => sql.MigrationsAssembly("FplDashboard.Migrations")
            )
        );

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FplDashboardDbContext>();
        db.Database.Migrate();

        Console.WriteLine("Migrations applied successfully.");
    }
}
