using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Interfaces;

public interface IIncomesRepository
{
    Task<Income?> GetByIdAsync(int id);
    Task<IEnumerable<Income>> GetAllAsync();
    Task<IEnumerable<Income>> GetIncomesByYearAndMonthAsync(int year, int month);
    Task<int> AddAsync(Income income);
    Task<bool> UpdateAsync(Income income);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByDescriptionInTheSameMonthAsync(string description, DateTime date);
}