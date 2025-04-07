using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.IncomeHandlers;

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, int>
{
    private readonly IIncomeRepository _repository;

    public CreateIncomeCommandHandler(IIncomeRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var Income = new Income
        {
            Description = request.Description,
            Value = request.Value,
            Date = request.Date
        };

        return await _repository.AddAsync(Income);
    }
}
