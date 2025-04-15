namespace PebaFinance.Domain.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
}
