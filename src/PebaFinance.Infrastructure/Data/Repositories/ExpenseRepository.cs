using System;
using Microsoft.EntityFrameworkCore;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data.Repositories;

public class ExpenseRepository : IExpenseRepository
    {
        private readonly FinanceDbContext _context;

        public ExpenseRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Expense Expense)
        {
            _context.Expense.Add(Expense);
            await _context.SaveChangesAsync();
            return Expense.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Expense = await _context.Expense.FindAsync(id);
            if (Expense == null) return false;

            _context.Expense.Remove(Expense);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            return await _context.Expense.ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _context.Expense.FindAsync(id);
        }

        public async Task<IEnumerable<Expense>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Expense
                .Where(d => d.Date >= startDate && d.Date <= endDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Expense Expense)
        {
            _context.Expense.Update(Expense);
            return await _context.SaveChangesAsync() > 0;
        }
    }