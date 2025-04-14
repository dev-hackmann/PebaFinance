using Dapper;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class IncomesRepository : IIncomesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public IncomesRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AddAsync(Income income)
    {
        try
        {
            const string sql = @"
            INSERT INTO income (Description, Value, Date) 
            VALUES (@Description, @Value, @Date);
            SELECT LAST_INSERT_ID();";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, income);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            const string sql = "DELETE FROM income WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Income>> GetAllAsync()
    {
        try
        {
            const string sql = "SELECT * FROM income";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Income>(sql);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<Income?> GetByIdAsync(int id)
    {
        try
        {
            const string sql = "SELECT * FROM income WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Income>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Income>> GetIncomeByYearAndMonthAsync(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            const string sql = @"
            SELECT * FROM income 
            WHERE Date >= @StartDate AND Date < @EndDate";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Income>(sql, new { StartDate = startDate, EndDate = endDate });
        }
        catch (Exception ex)
        {
            throw new Exception("Error accessing the database", ex);
        }
    }

    public async Task<bool> UpdateAsync(Income income)
    {
        try
        {
            const string sql = @"
            UPDATE income 
            SET Description = @Description, Value = @Value, Date = @Date
            WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, income);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<bool> ExistsByDescriptionInTheSameMonthAsync(string description, DateTime date)
    {
        try
        {
            int year = date.Year;
            int month = date.Month;

            const string sql = @"
            SELECT COUNT(1)
            FROM income
            WHERE Description = @Description
                AND YEAR(Date) = @Year 
                AND MONTH(Date) = @Month";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new { Description = description, Year = year, Month = month }) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }
}