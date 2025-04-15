using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class GetIncomesByYearAndMonthQueryHandler : IRequestHandler<GetIncomesByYearAndMonthQuery, IEnumerable<IncomeDto>>
{
    private readonly IIncomesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetIncomesByYearAndMonthQueryHandler(IIncomesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public async Task<IEnumerable<IncomeDto>> Handle(GetIncomesByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var Incomes = await _repository.GetIncomeByYearAndMonthAsync(request.year, request.month, userId);

            return Incomes.Select(Income => new IncomeDto
            {
                Id = Income.Id,
                Description = Income.Description,
                Value = Income.Value,
                Date = Income.Date,
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while getting the income.", ex);
        }
    }
}