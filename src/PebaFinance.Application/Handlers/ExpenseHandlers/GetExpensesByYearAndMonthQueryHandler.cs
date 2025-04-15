using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class GetExpensesByYearAndMonthQueryHandler : IRequestHandler<GetExpensesByYearAndMonthQuery, IEnumerable<ExpenseDto>>
{
    private readonly IExpensesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetExpensesByYearAndMonthQueryHandler(IExpensesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(GetExpensesByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var expenses = await _repository.GetExpensesByYearAndMonthAsync(request.year, request.month, userId);
        
        return expenses.Select(expense => new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Value = expense.Value,
            Date = expense.Date,
            Category = expense.Category.ToString(),
        });
    }
}