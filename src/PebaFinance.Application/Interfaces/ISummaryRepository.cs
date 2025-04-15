using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface ISummaryRepository
{
    Task<IEnumerable<SummaryExpensesByCategory>> GetSummaryByYearAndMonthAsync(int year, int month, int userId);
}
