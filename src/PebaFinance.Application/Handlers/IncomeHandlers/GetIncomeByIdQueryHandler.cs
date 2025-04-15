using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class GetIncomesByIdQueryHandler : IRequestHandler<GetIncomeByIdQuery, IncomeDto?>
{
    private readonly IIncomesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetIncomesByIdQueryHandler(IIncomesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public async Task<IncomeDto?> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var income = await _repository.GetByIdAsync(request.Id, userId);
            if (income == null) return null;

            return new IncomeDto
            {
                Id = income.Id,
                Description = income.Description,
                Value = income.Value,
                Date = income.Date
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while getting the income.", ex);
        }
    }
}
