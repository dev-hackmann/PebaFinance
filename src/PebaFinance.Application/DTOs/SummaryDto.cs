namespace PebaFinance.Application.DTOs;

public class SummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal FinalBalance { get; set; }
    public List<SummaryByCategoryDto> SummaryByCategory { get; set; } = new();
}

public class SummaryByCategoryDto
{
    public string Category { get; set; }
    public decimal Total { get; set; }
}
