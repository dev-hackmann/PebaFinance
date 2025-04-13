using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Enums;
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

        ExpenseCategory? expenseCategory = null;
        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            if (Enum.TryParse<ExpenseCategory>(request.Category, true, out var parsedCategory))
            {
                expenseCategory = parsedCategory;
            }
            else
            {
                throw new InvalidCategoryException(request.Category);
            }
        }

        var expense = new Expense(request.Description, request.Value, request.Date, expenseCategory);

        return await _repository.AddAsync(expense);
    }
}
