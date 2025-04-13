using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;

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
        var expense = await _repository.GetByIdAsync(request.Id);
        if (expense == null) return false;

        expense.Description = request.Description;
        expense.Value = request.Value;
        expense.Date = request.Date;

        if (await _repository.ExistsByDescriptionInTheSameMonthAsync(expense.Description, expense.Date))
        {
            throw new DuplicateDescriptionException(expense.Description, expense.Date);
        }

        return await _repository.UpdateAsync(expense);
    }
}