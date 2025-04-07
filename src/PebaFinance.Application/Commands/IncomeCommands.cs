using MediatR;

namespace PebaFinance.Application.Commands;

public record CreateIncomeCommand(string Description, decimal Value, DateTime Date) : IRequest<int>;
public record UpdateIncomeCommand(int Id, string Description, decimal Value, DateTime Date) : IRequest<bool>;
public record DeleteIncomeCommand(int Id) : IRequest<bool>;