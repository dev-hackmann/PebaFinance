using Microsoft.EntityFrameworkCore;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly FinanceDbContext _context;

    public IncomeRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Income Income)
    {
        _context.Income.Add(Income);
        await _context.SaveChangesAsync();
        return Income.Id;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var Income = await _context.Income.FindAsync(id);
        if (Income == null) return false;

        _context.Income.Remove(Income);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Income>> GetAllAsync()
    {
        return await _context.Income.ToListAsync();
    }

    public async Task<Income?> GetByIdAsync(int id)
    {
        return await _context.Income.FindAsync(id);
    }

    public async Task<IEnumerable<Income>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Income
            .Where(r => r.Date >= startDate && r.Date <= endDate)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Income Income)
    {
        _context.Income.Update(Income);
        return await _context.SaveChangesAsync() > 0;
    }
}
