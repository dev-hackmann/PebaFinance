using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class GetIncomesByYearAndMonthQueryHandler : IRequestHandler<GetIncomesByYearAndMonthQuery, IEnumerable<IncomeDto>>
{
    private readonly IIncomesRepository _repository;

    public GetIncomesByYearAndMonthQueryHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IncomeDto>> Handle(GetIncomesByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        var Incomes = await _repository.GetIncomesByYearAndMonthAsync(request.year, request.month);
        return Incomes.Select(Income => new IncomeDto
        {
            Id = Income.Id,
            Description = Income.Description,
            Value = Income.Value,
            Date = Income.Date,
        });
    }
}