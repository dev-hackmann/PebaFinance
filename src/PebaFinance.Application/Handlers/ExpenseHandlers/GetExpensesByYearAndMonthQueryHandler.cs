using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class GetExpensesByYearAndMonthQueryHandler : IRequestHandler<GetExpensesByYearAndMonthQuery, IEnumerable<ExpenseDto>>
{
    private readonly IExpensesRepository _repository;

    public GetExpensesByYearAndMonthQueryHandler(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(GetExpensesByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetExpensesByYearAndMonthAsync(request.year, request.month);
        return expenses.Select(expense => new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Value = expense.Value,
            Date = expense.Date,
            Category = expense.Category.ToString(),
        });
    }
}