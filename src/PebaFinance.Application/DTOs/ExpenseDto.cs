using PebaFinance.Domain.Enums;

namespace PebaFinance.Application.DTOs;

public class ExpenseDto : BaseDto
{
    public string Category { get; set; }
}