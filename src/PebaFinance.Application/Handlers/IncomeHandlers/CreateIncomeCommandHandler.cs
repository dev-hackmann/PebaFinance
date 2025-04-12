using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class CreateIncomesCommandHandler : IRequestHandler<CreateIncomeCommand, int>
{
    private readonly IIncomesRepository _repository;

    public CreateIncomesCommandHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = new Income
        {
            Description = request.Description,
            Value = request.Value,
            Date = request.Date
        };

        return await _repository.AddAsync(income);
    }
}
