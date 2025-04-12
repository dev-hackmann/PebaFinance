using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class DeleteExpensesCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpensesRepository _repository;

    public DeleteExpensesCommandHandler(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id);
    }
}