using System.Data;
using Microsoft.Data.SqlClient;

namespace FplDashboard.API.Factories;

public class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("FplDashboard");
        return new SqlConnection(connectionString);
    }
}