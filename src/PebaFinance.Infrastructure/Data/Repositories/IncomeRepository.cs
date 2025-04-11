// src/PebaFinance.Infrastructure/Data/Repositories/IncomeRepository.cs
using System.Data;
using Dapper;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public IncomeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AddAsync(Income income)
    {
        const string sql = @"
            INSERT INTO income (Description, Value, Date) 
            VALUES (@Description, @Value, @Date);
            SELECT LAST_INSERT_ID();";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, income);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM income WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Income>> GetAllAsync()
    {
        const string sql = "SELECT * FROM income";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Income>(sql);
    }

    public async Task<Income?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM income WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Income>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Income>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        const string sql = "SELECT * FROM income WHERE Date >= @StartDate AND Date <= @EndDate";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Income>(sql, new { StartDate = startDate, EndDate = endDate });
    }

    public async Task<bool> UpdateAsync(Income income)
    {
        const string sql = @"
            UPDATE income 
            SET Description = @Description, Value = @Value, Date = @Date
            WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, income);
        return rowsAffected > 0;
    }
}