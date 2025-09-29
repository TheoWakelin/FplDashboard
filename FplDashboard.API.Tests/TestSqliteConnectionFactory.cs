using System.Data;
using FplDashboard.API.Infrastructure;
using Microsoft.Data.Sqlite;

namespace FplDashboard.API.Tests;

public class TestSqliteConnectionFactory : IDbConnectionFactory
{
    private readonly SqliteConnection _connection;
    public TestSqliteConnectionFactory(SqliteConnection connection)
    {
        _connection = connection;
    }
    public IDbConnection CreateConnection() => _connection;
}