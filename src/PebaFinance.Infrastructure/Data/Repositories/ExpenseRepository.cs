using System.Data;
using Dapper;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ExpenseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AddAsync(Expense expense)
    {
        const string sql = @"
            INSERT INTO expense (Description, Value, Date) 
            VALUES (@Description, @Value, @Date);
            SELECT LAST_INSERT_ID();";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, expense);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM expense WHERE Id = @Id";
        
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        const string sql = "SELECT * FROM expense";
        
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Expense>(sql);
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM expense WHERE Id = @Id";
        
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Expense>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Expense>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        const string sql = "SELECT * FROM expense WHERE Date >= @StartDate AND Date <= @EndDate";
        
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Expense>(sql, new { StartDate = startDate, EndDate = endDate });
    }

    public async Task<bool> UpdateAsync(Expense expense)
    {
        const string sql = @"
            UPDATE expense 
            SET Description = @Description, Value = @Value, Date = @Date
            WHERE Id = @Id";
        
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, expense);
        return rowsAffected > 0;
    }
}