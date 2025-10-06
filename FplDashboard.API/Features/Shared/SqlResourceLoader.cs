using System.Reflection;

namespace FplDashboard.API.Features.Shared;

public static class SqlResourceLoader
{
    public static async Task<string> GetSql(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        await using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
            throw new InvalidOperationException($"Embedded SQL resource not found: {resourcePath}");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
