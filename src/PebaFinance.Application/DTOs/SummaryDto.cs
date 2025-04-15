namespace PebaFinance.Application.DTOs;

public class SummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal FinalBalance { get; set; }
    public List<SummaryExpensesByCategoryDto> SummaryExpensesByCategory { get; set; } = new();
}

public class SummaryExpensesByCategoryDto
{
    public string Category { get; set; }
    public decimal Total { get; set; }
}
