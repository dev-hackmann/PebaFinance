using System;
using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class CreateExpensesCommandHandler : IRequestHandler<CreateExpenseCommand, int>
{
    private readonly IExpensesRepository _repository;

    public CreateExpensesCommandHandler(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByDescriptionInTheSameMonthAsync(request.Description, request.Date))
        {
            throw new DuplicateDescriptionException(request.Description, request.Date);
        }

        var expense = new Expense
        {
            Description = request.Description,
            Value = request.Value,
            Date = request.Date
        };

        return await _repository.AddAsync(expense);
    }
}
