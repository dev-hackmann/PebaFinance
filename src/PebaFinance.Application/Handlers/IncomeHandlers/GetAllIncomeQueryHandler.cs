using System;
using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomeHandlers;

public class GetAllIncomeQueryHandler : IRequestHandler<GetAllIncomesQuery, IEnumerable<IncomeDto>>
{
    private readonly IIncomeRepository _repository;

    public GetAllIncomeQueryHandler(IIncomeRepository respository)
    {
        _repository = respository;
    }

    public async Task<IEnumerable<IncomeDto>> Handle(GetAllIncomesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetAllAsync();
        return expenses.Select(income => new IncomeDto
        {
            Id = income.Id,
            Description = income.Description,
            Value = income.Value,
            Date = income.Date
        });
    }
}
