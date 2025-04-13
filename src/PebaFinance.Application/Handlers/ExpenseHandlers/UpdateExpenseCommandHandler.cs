using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Enums;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class UpdateExpensesCommandHandler : IRequestHandler<UpdateExpenseCommand, bool>
{
    private readonly IExpensesRepository _repository;

    public UpdateExpensesCommandHandler(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.GetByIdAsync(request.Id) == null) return false;

        if (await _repository.ExistsByDescriptionInTheSameMonthWithDifferentIdAsync(request.Id, request.Description, request.Date))
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

        var expense = new Expense(request.Description, request.Value, request.Date, expenseCategory)
        {
            Id = request.Id
        };

        return await _repository.UpdateAsync(expense);
    }
}