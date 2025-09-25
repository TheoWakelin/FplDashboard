using System.Data;

namespace FplDashboard.API.Factories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

