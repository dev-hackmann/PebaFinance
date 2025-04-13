using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class DeleteIncomesCommandHandler : IRequestHandler<DeleteIncomeCommand, bool>
{
    private readonly IIncomesRepository _repository;

    public DeleteIncomesCommandHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id);
    }
}