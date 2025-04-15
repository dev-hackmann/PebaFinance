namespace PebaFinance.Application.DTOs;

public class LoginResponseDto
{
    public int UserId { get; }
    public string Token { get; }

    public LoginResponseDto(int userId, string token)
    {
        UserId = userId;
        Token = token;
    }
}
