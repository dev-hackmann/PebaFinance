using System.Text.Json.Serialization;
using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<ExpenseDto?>;
public record GetAllExpensesQuery(BaseFilterDto filter) : IRequest<IEnumerable<ExpenseDto>>
{
    [JsonIgnore]
    public int UserId { get; set; }
}
public record GetExpensesByYearAndMonthQuery(int year, int month) : IRequest<IEnumerable<ExpenseDto>>;