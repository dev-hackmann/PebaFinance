using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpenseHandlers;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
{
    private readonly IExpensesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetExpenseByIdQueryHandler(IExpensesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var Expense = await _repository.GetByIdAsync(request.Id, userId);
        if (Expense == null) return null;

        return new ExpenseDto
        {
            Id = Expense.Id,
            Description = Expense.Description,
            Value = Expense.Value,
            Date = Expense.Date,
            Category = Expense.Category.ToString()
        };
    }
}
