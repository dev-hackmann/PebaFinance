using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class GetAllIncomesQueryHandler : IRequestHandler<GetAllIncomesQuery, IEnumerable<IncomeDto>>
{
    private readonly IIncomesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllIncomesQueryHandler(IIncomesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<IncomeDto>> Handle(GetAllIncomesQuery request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var incomes = await _repository.GetAllAsync(userId);

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
