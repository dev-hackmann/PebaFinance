using PebaFinance.Domain.Enums;

namespace PebaFinance.Domain.Models;

public class Expense : BaseEntity
{
    public ExpenseCategory Category { get; private set; }

    public Expense() { }

    public Expense(string description, decimal value, DateTime date, int userId, ExpenseCategory? category = null)
    {
        Description = description;
        Value = value;
        Date = date;
        Category = category ?? ExpenseCategory.Others;
        UserId = userId;
    }
}