using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.SummaryHandlers;

public class GetSummaryByYearAndMonthQueryHandler : IRequestHandler<GetSummaryByYearAndMonthQuery, SummaryDto>
{
    private readonly IExpensesRepository _repositoryExpenses;
    private readonly IIncomesRepository _repositoryIncome;
    private readonly ISummaryRepository _repositorySummary;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetSummaryByYearAndMonthQueryHandler(IExpensesRepository repositoryExpenses, IIncomesRepository repositoryIncome, ISummaryRepository repositorySummary, IHttpContextAccessor httpContextAccessor)
    {
        _repositoryExpenses = repositoryExpenses;
        _repositoryIncome = repositoryIncome;
        _repositorySummary = repositorySummary;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SummaryDto> Handle(GetSummaryByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var expense = await _repositoryExpenses.GetExpensesByYearAndMonthAsync(request.year, request.month, userId);
        var income = await _repositoryIncome.GetIncomeByYearAndMonthAsync(request.year, request.month, userId);
        var summary = await _repositorySummary.GetSummaryByYearAndMonthAsync(request.year, request.month, userId);

        var totalExpenses = expense.Sum(e => e.Value);
        var totalIncome = income.Sum(i => i.Value);
        var finalBalance = totalIncome - totalExpenses;

        var summaryByCategoryDto = summary
        .Select(s => new SummaryExpensesByCategoryDto
        {
            Category = s.Category.ToString(),
            Total = s.Total
        })
        .ToList();

        return new SummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            FinalBalance = finalBalance,
            SummaryExpensesByCategory = summaryByCategoryDto
        };
    }
}
