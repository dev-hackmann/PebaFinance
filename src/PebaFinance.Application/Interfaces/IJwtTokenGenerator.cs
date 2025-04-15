using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
