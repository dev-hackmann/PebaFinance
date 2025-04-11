using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpenseHandlers;

public class GetAllExpenseQueryHandler : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;

    public GetAllExpenseQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetAllAsync();
        return expenses.Select(expense => new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Value = expense.Value,
            Date = expense.Date
        });
    }
}