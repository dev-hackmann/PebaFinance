using MediatR;

namespace PebaFinance.Application.Commands;

public record CreateExpenseCommand(string Description, decimal Value, DateTime Date) : IRequest<int>;
public record UpdateExpenseCommand(int Id, string Description, decimal Value, DateTime Date) : IRequest<bool>;
public record DeleteExpenseCommand(int Id) : IRequest<bool>;