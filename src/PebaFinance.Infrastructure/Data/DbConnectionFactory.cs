using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace PebaFinance.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private IDbConnection? _connection;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public IDbConnection CreateConnection()
    {
        if (_connection == null || _connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
        {
            _connection = new MySqlConnection(_connectionString);
        }

        return _connection;
    }
}