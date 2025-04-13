using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IExpensesRepository
{
    Task<Expense?> GetByIdAsync(int id);
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<IEnumerable<Expense>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> AddAsync(Expense expense);
    Task<bool> UpdateAsync(Expense expense);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByDescriptionInTheSameMonthAsync(string description, DateTime date);
    Task<bool> ExistsByDescriptionInTheSameMonthWithDifferentIdAsync(int id, string description, DateTime date);
}
