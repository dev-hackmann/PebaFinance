using Dapper;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class SummaryRepository : ISummaryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SummaryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<SummaryExpensesByCategory>> GetSummaryByYearAndMonthAsync(int year, int month, int userId)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            const string sql = @"
            SELECT 
                category, 
                SUM(value) AS total
            FROM expense 
            WHERE date >= @StartDate 
                AND date < @EndDate
                AND user_id = @UserId
            GROUP BY category;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<SummaryExpensesByCategory>(sql, new { StartDate = startDate, EndDate = endDate, UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }
}