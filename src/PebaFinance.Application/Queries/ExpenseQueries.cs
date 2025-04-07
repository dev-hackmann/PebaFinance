using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<ExpenseDto?>;
public record GetAllExpensesQuery : IRequest<IEnumerable<ExpenseDto>>;
public record GetExpensesByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<ExpenseDto>>;