using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.AuthenticateUserHandlers;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthenticateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResponseDto> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
                throw new UnauthorizedAccessException("Email or password are incorrect.");

            var token = _tokenGenerator.GenerateToken(user);
            return new LoginResponseDto(user.Id, token);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while authenticating the user.", ex);
        }
    }
}
