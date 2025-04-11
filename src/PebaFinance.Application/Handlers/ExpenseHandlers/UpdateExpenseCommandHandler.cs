using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.ExpenseHandlers;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, bool>
{
    private readonly IExpenseRepository _repository;

    public UpdateExpenseCommandHandler(IExpenseRepository repository)
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

        return await _repository.UpdateAsync(expense);
    }
}