using System.Text.Json.Serialization;
using MediatR;

namespace PebaFinance.Application.Commands;

public interface IIncomeCommand
{
    string Description { get; }
    decimal Value { get; }
    DateTime Date { get; }
}

public record CreateIncomeCommand(string Description, decimal Value, DateTime Date) : IRequest<int>, IIncomeCommand;
public record UpdateIncomeCommand(string Description, decimal Value, DateTime Date) : IRequest<bool>, IIncomeCommand
{
    [JsonIgnore]
    public int Id { get; set; }
}
public record DeleteIncomeCommand(int Id) : IRequest<bool>;