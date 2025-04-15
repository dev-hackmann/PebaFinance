using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new EmailAlreadyRegisteredException();
            }

            var hashedPassword = _passwordHasher.Hash(request.Password);

            var newUser = new User(request.Name, request.Email, hashedPassword);
            await _userRepository.AddAsync(newUser);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while registering the user.", ex);
        }
    }
}
