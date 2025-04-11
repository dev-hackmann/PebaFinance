using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;

namespace PebaFinance.Application.Handlers.ExpenseHandlers;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpenseRepository _repository;

    public DeleteExpenseCommandHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id);
    }
}