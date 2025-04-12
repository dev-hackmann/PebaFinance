using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpenseHandlers;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
{
    private readonly IExpensesRepository _repository;

    public GetExpenseByIdQueryHandler(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var Expense = await _repository.GetByIdAsync(request.Id);
        if (Expense == null) return null;

        return new ExpenseDto
        {
            Id = Expense.Id,
            Description = Expense.Description,
            Value = Expense.Value,
            Date = Expense.Date
        };
    }
}
