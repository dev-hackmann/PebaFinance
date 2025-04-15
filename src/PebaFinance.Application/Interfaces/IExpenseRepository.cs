using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IExpensesRepository
{
    Task<Expense?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<Expense>> GetAllAsync(int userId);
    Task<IEnumerable<Expense>> GetExpensesByYearAndMonthAsync(int year, int month, int userId);
    Task<int> AddAsync(Expense expense);
    Task<bool> UpdateAsync(Expense expense);
    Task<bool> DeleteAsync(int id, int userId);
    Task<bool> ExistsByDescriptionInTheSameMonthAsync(string description, DateTime date, int userId);
    Task<bool> ExistsByDescriptionInTheSameMonthWithDifferentIdAsync(int id, string description, DateTime date, int userId);
}