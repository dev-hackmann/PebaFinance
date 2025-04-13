using System.Text.Json.Serialization;
using MediatR;

namespace PebaFinance.Application.Commands;

public interface IExpenseCommand
{
    string Description { get; }
    decimal Value { get; }
    DateTime Date { get; }
}

public record CreateExpenseCommand(string Description, decimal Value, DateTime Date, string? Category) : IRequest<int>, IExpenseCommand;
public record UpdateExpenseCommand(string Description, decimal Value, DateTime Date) : IRequest<bool>, IExpenseCommand
{
    [JsonIgnore]
    public int Id { get; set; }
}
public record DeleteExpenseCommand(int Id) : IRequest<bool>;