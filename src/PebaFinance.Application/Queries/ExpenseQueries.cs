using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<ExpenseDto?>;
public record GetAllExpensesQuery : IRequest<IEnumerable<ExpenseDto>>;