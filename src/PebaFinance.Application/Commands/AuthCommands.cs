using MediatR;

namespace PebaFinance.Application.Commands;

public record RegisterUserCommand(string Email, string Password, string Name, string ConfirmPassword) : IRequest<bool>;