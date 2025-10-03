using System.Reflection;

namespace FplDashboard.API.Features.Shared;

public static class SqlResourceLoader
{
    public static string GetSql(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
            throw new InvalidOperationException($"Embedded SQL resource not found: {resourcePath}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
