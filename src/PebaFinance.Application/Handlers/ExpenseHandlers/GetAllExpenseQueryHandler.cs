using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseDto>>
{
    private readonly IExpensesRepository _repository;

    public GetAllExpensesQueryHandler(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetAllAsync();

        if (request.filter.description != null)
        {
            expenses = expenses.Where(expense => expense.Description.Contains(request.filter.description, StringComparison.OrdinalIgnoreCase));
        }

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