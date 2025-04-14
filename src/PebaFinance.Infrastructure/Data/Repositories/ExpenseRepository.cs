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
            INSERT INTO expense (Description, Value, Date, Category) 
            VALUES (@Description, @Value, @Date, @Category);
            SELECT LAST_INSERT_ID();";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, expense);
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
            const string sql = "DELETE FROM expense WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        try
        {
            const string sql = "SELECT * FROM expense";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Expense>(sql);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        try
        {
            const string sql = "SELECT * FROM expense WHERE Id = @Id";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Expense>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<IEnumerable<Expense>> GetExpensesByYearAndMonthAsync(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            const string sql = @"
            SELECT * FROM expense 
            WHERE Date >= @StartDate AND Date < @EndDate";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Expense>(sql, new { StartDate = startDate, EndDate = endDate });
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
            SET Description = @Description, Value = @Value, Date = @Date, Category = @Category
            WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, expense);
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
            FROM expense
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

    public async Task<bool> ExistsByDescriptionInTheSameMonthWithDifferentIdAsync(int id, string description, DateTime date)
    {
        try
        {
            int year = date.Year;
            int month = date.Month;

            const string sql = @"
            SELECT COUNT(1)
            FROM expense
            WHERE Description = @Description
                AND YEAR(Date) = @Year 
                AND MONTH(Date) = @Month
                AND Id != @Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new { Id = id, Description = description, Year = year, Month = month }) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }
}