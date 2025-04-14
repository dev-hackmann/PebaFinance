using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface ISummaryRepository
{
    Task<IEnumerable<SummaryByCategory>> GetSummaryByYearAndMonthAsync(int year, int month);
}
