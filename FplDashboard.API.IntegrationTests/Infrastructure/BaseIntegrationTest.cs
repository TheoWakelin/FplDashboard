namespace FplDashboard.API.IntegrationTests.Infrastructure;

[Collection("Database collection")]
public abstract class BaseIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncDisposable
{
    protected readonly DatabaseFixture Fixture;
    protected ApiTestClient ApiClient => new(_client);
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    protected BaseIntegrationTest(DatabaseFixture fixture)
    {
        Fixture = fixture;
        _factory = new TestWebApplicationFactory(fixture.ConnectionString);
        _client = _factory.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        await _factory.DisposeAsync();
    }
}
