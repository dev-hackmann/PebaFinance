using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IIncomesRepository
{
    Task<Income?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<Income>> GetAllAsync(int userId);
    Task<IEnumerable<Income>> GetIncomeByYearAndMonthAsync(int year, int month, int userId);
    Task<int> AddAsync(Income income);
    Task<bool> UpdateAsync(Income income);
    Task<bool> DeleteAsync(int id, int userId);
    Task<bool> ExistsByDescriptionInTheSameMonthAsync(string description, DateTime date, int userId);
}