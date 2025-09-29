using System.Data;

namespace FplDashboard.API.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

