using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class GetIncomesByIdQueryHandler : IRequestHandler<GetIncomeByIdQuery, IncomeDto?>
{
    private readonly IIncomesRepository _repository;

    public GetIncomesByIdQueryHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IncomeDto?> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
    {
        var income = await _repository.GetByIdAsync(request.Id);
        if (income == null) return null;

        return new IncomeDto
        {
            Id = income.Id,
            Description = income.Description,
            Value = income.Value,
            Date = income.Date
        };
    }
}
