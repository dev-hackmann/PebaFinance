using System.Data;
using Dapper;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class ExpensesRepository : IExpensesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ExpensesRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AddAsync(Expense expense)
    {
        try
        {
            const string sql = @"
            INSERT INTO expense (description, value, date, category, user_id) 
            VALUES (@Description, @Value, @Date, @Category, @UserId);
            SELECT LAST_INSERT_ID();";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, expense);
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
            const string sql = "DELETE FROM expense WHERE id = @Id AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(int userId)
    {
        try
        {
            const string sql = "SELECT * FROM expense WHERE user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Expense>(sql, new { UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<Expense?> GetByIdAsync(int id, int userId)
    {
        try
        {
            const string sql = "SELECT * FROM expense WHERE id = @Id AND user_id = @UserId";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Expense>(sql, new { Id = id, UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Expense>> GetExpensesByYearAndMonthAsync(int year, int month, int userId)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            const string sql = @"
            SELECT * FROM expense 
            WHERE date >= @StartDate AND date < @EndDate
                AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Expense>(sql, new { StartDate = startDate, EndDate = endDate, UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception("Error accessing the database", ex);
        }
    }

    public async Task<bool> UpdateAsync(Expense expense)
    {
        try
        {
            const string sql = @"
            UPDATE expense 
            SET description = @Description, value = @Value, date = @Date, category = @Category
            WHERE id = @Id
                 AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, expense);
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
            FROM expense
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

    public async Task<bool> ExistsByDescriptionInTheSameMonthWithDifferentIdAsync(int id, string description, DateTime date, int userId)
    {
        try
        {
            int year = date.Year;
            int month = date.Month;

            const string sql = @"
            SELECT COUNT(1)
            FROM expense
            WHERE description = @Description
                AND YEAR(date) = @Year 
                AND MONTH(date) = @Month
                AND id != @Id
                AND user_id = @UserId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new { Id = id, Description = description, Year = year, Month = month, UserId = userId }) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }
}