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
            INSERT INTO income (description, value, date, user_id) 
            VALUES (@Description, @Value, @Date, @UserId);
            SELECT LAST_INSERT_ID();";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, income);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        try
        {
            const string sql = "DELETE FROM income WHERE id = @Id AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Income>> GetAllAsync(int userId)
    {
        try
        {
            const string sql = "SELECT * FROM income WHERE user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Income>(sql, new { UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<Income?> GetByIdAsync(int id, int userId)
    {
        try
        {
            const string sql = "SELECT * FROM income WHERE id = @Id AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Income>(sql, new { Id = id, UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Income>> GetIncomeByYearAndMonthAsync(int year, int month, int userId)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            const string sql = @"
            SELECT * FROM income 
            WHERE date >= @StartDate AND date < @EndDate
                AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Income>(sql, new { StartDate = startDate, EndDate = endDate, UserId = userId });
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
            SET description = @Description, value = @Value, date = @Date
            WHERE id = @Id 
                AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, income);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<bool> ExistsByDescriptionInTheSameMonthAsync(string description, DateTime date, int userId)
    {
        try
        {
            int year = date.Year;
            int month = date.Month;

            const string sql = @"
            SELECT COUNT(1)
            FROM income
            WHERE description = @Description
                AND YEAR(date) = @Year 
                AND MONTH(date) = @Month
                AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new { Description = description, Year = year, Month = month, UserId = userId }) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }
}