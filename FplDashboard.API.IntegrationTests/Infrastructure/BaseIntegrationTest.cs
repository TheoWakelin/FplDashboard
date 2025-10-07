namespace FplDashboard.API.IntegrationTests.Infrastructure;

[Collection("Database collection")]
public abstract class BaseIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncDisposable
{
    protected readonly DatabaseFixture Fixture;
    protected readonly ApiTestClient ApiClient;
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    protected BaseIntegrationTest(DatabaseFixture fixture)
    {
        Fixture = fixture;
        _factory = new TestWebApplicationFactory(fixture.ConnectionString);
        _client = _factory.CreateClient();
        ApiClient = new ApiTestClient(_client);
    }

    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        await _factory.DisposeAsync();
    }
}
