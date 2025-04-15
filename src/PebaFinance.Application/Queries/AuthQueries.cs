using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record AuthenticateUserQuery(string Email, string Password) : IRequest<LoginResponseDto>;