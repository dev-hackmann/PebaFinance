using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.ExpenseHandlers;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, int>
{
    private readonly IExpenseRepository _repository;

    public CreateExpenseCommandHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var Expense = new Expense
        {
            Description = request.Description,
            Value = request.Value,
            Date = request.Date
        };

        return await _repository.AddAsync(Expense);
    }
}
