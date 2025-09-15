using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FplDashboard.DataModel;

public class FplDashboardDbContextFactory : IDesignTimeDbContextFactory<FplDashboardDbContext>
{
    // Design-time method for migrations
    public FplDashboardDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FplDashboardDbContext>();
        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("FplDashboard"),
            b => b.MigrationsAssembly("FplDashboard.Migrations")
        );

        return new FplDashboardDbContext(optionsBuilder.Options);
    }
}
