using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<ExpenseDto?>;
public record GetAllExpensesQuery(BaseFilterDto filter) : IRequest<IEnumerable<ExpenseDto>>;
public record GetExpensesByYearAndMonthQuery(int year, int month) : IRequest<IEnumerable<ExpenseDto>>;