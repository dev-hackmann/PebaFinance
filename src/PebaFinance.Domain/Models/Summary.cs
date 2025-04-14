using PebaFinance.Domain.Enums;

namespace PebaFinance.Domain.Models;

public class Summary
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal FinalBalance { get; set; }
    public List<SummaryByCategory> SummaryByCategory { get; set; } = new();
}

public class SummaryByCategory
{
    public ExpenseCategory Category { get; set; }
    public decimal Total { get; set; }
}
