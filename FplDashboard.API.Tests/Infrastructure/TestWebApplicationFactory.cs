using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace FplDashboard.API.Tests.Infrastructure;

public class TestWebApplicationFactory(string testConnectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Override the connection string for tests
            config.AddInMemoryCollection([
                new KeyValuePair<string, string?>("ConnectionStrings:FplDashboard", testConnectionString)
            ]);
        });
    }
}
