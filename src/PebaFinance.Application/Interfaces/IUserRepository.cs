using System;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<int> AddAsync(User user);
}
