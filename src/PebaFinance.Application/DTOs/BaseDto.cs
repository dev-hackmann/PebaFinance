namespace PebaFinance.Application.DTOs;

public class BaseDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}
