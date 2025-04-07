using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int id);
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<IEnumerable<Expense>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> AddAsync(Expense Expense);
    Task<bool> UpdateAsync(Expense Expense);
    Task<bool> DeleteAsync(int id);
}
