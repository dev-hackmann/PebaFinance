using System.Data;
using MySql.Data.MySqlClient;
using PebaFinance.Infrastructure.Data;

namespace PebaFinance.IntegrationTests.Infrastructure;

public class DatabaseFixture : IDisposable, IDbConnectionFactory
{
    private readonly MySqlConnection _connection;

    public DatabaseFixture()
    {
        _connection = new MySqlConnection("Server=peba-finance-db;Port=3306;Database=peba_finance;User=root;Password=abcv123;");
        _connection.Open();

        using var command = _connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS `expense` (
                `Id` INT NOT NULL AUTO_INCREMENT,
                `Description` VARCHAR(200) NOT NULL,
                `Value` DECIMAL(18,2) NOT NULL,
                `Date` DATE NOT NULL,
                PRIMARY KEY (`Id`)
            );";
        command.ExecuteNonQuery();
    }

    public IDbConnection CreateConnection()
    {
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}