using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IIncomeRepository
{
    Task<Income?> GetByIdAsync(int id);
    Task<IEnumerable<Income>> GetAllAsync();
    Task<IEnumerable<Income>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> AddAsync(Income Income);
    Task<bool> UpdateAsync(Income Income);
    Task<bool> DeleteAsync(int id);
}