using System.Net.Http.Json;

namespace FplDashboard.API.IntegrationTests.Infrastructure;

public class ApiTestClient(HttpClient httpClient)
{
    public async Task<T> PostAndExpectSuccessAsync<T>(string endpoint, object request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);
        return await MakeAssertionsAndReadAsType<T>(response, cancellationToken);
    }

    public async Task<T> GetAndExpectSuccessAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(endpoint, cancellationToken);
        return await MakeAssertionsAndReadAsType<T>(response, cancellationToken);
    }
    
    private static async Task<T> MakeAssertionsAndReadAsType<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        Assert.NotNull(result);
        return result;
    }
}
