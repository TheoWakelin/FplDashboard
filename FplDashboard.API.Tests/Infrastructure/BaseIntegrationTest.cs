namespace FplDashboard.API.Tests.Infrastructure;

[Collection("Database collection")]
public abstract class BaseIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncDisposable
{
    protected readonly DatabaseFixture Fixture;
    protected readonly HttpClient Client;
    private readonly TestWebApplicationFactory _factory;

    protected BaseIntegrationTest(DatabaseFixture fixture)
    {
        Fixture = fixture;
        _factory = new TestWebApplicationFactory(fixture.ConnectionString);
        Client = _factory.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        Client?.Dispose();
        await _factory.DisposeAsync();
    }
}
