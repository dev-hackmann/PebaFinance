using Dapper;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            const string sql = @"
            SELECT 
                id as Id, 
                name as Name, 
                email as Email, 
                password_hash as PasswordHash
            FROM user 
            WHERE email = @Email";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }

    public async Task<int> AddAsync(User user)
    {
        try
        {
            const string sql = @"
            INSERT INTO user (name, email, password_hash) 
            VALUES (@Name, @Email, @PasswordHash);
            SELECT LAST_INSERT_ID();";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error accessing the database", ex);
        }
    }
}