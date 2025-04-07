using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomeHandlers;

public class GetIncomeByIdQueryHandler : IRequestHandler<GetIncomeByIdQuery, IncomeDto?>
{
    private readonly IIncomeRepository _repository;

    public GetIncomeByIdQueryHandler(IIncomeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IncomeDto?> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
    {
        var Income = await _repository.GetByIdAsync(request.Id);
        if (Income == null) return null;

        return new IncomeDto
        {
            Id = Income.Id,
            Description = Income.Description,
            Value = Income.Value,
            Date = Income.Date
        };
    }
}
