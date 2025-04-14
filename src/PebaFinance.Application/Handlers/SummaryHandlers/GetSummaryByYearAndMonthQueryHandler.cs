using MediatR;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.SummaryHandlers;

public class GetSummaryByYearAndMonthQueryHandler : IRequestHandler<GetSummaryByYearAndMonthQuery, SummaryDto>
{
    private readonly IExpensesRepository _repositoryExpenses;
    private readonly IIncomesRepository _repositoryIncome;
    private readonly ISummaryRepository _repositorySummary;

    public GetSummaryByYearAndMonthQueryHandler(IExpensesRepository repositoryExpenses, IIncomesRepository repositoryIncome, ISummaryRepository repositorySummary)
    {
        _repositoryExpenses = repositoryExpenses;
        _repositoryIncome = repositoryIncome;
        _repositorySummary = repositorySummary;
    }

    public async Task<SummaryDto> Handle(GetSummaryByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        var expense = await _repositoryExpenses.GetExpensesByYearAndMonthAsync(request.year, request.month);
        var income = await _repositoryIncome.GetIncomeByYearAndMonthAsync(request.year, request.month);
        var summary = await _repositorySummary.GetSummaryByYearAndMonthAsync(request.year, request.month);

        var totalExpenses = expense.Sum(e => e.Value);
        var totalIncome = income.Sum(i => i.Value);
        var finalBalance = totalIncome - totalExpenses;

        var summaryByCategoryDto = summary
        .Select(s => new SummaryByCategoryDto
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
            SummaryByCategory = summaryByCategoryDto
        };
    }
}
