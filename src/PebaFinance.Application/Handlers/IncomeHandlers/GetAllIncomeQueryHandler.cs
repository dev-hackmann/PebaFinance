using System;
using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class GetAllIncomesQueryHandler : IRequestHandler<GetAllIncomesQuery, IEnumerable<IncomeDto>>
{
    private readonly IIncomesRepository _repository;

    public GetAllIncomesQueryHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IncomeDto>> Handle(GetAllIncomesQuery request, CancellationToken cancellationToken)
    {
        var incomes = await _repository.GetAllAsync();

        if (request.filter.description != null)
        {
            incomes = incomes.Where(expense => expense.Description.Contains(request.filter.description, StringComparison.OrdinalIgnoreCase));
        }

        return incomes.Select(income => new IncomeDto
        {
            Id = income.Id,
            Description = income.Description,
            Value = income.Value,
            Date = income.Date
        });
    }
}
