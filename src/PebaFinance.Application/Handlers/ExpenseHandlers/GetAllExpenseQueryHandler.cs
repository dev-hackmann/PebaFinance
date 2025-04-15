using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Interfaces;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseDto>>
{
    private readonly IExpensesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllExpensesQueryHandler(IExpensesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var expenses = await _repository.GetAllAsync(userId);

            if (request.filter.description != null)
            {
                expenses = expenses.Where(expense => expense.Description.Contains(request.filter.description, StringComparison.OrdinalIgnoreCase));
            }

            return expenses.Select(expense => new ExpenseDto
            {
                Id = expense.Id,
                Description = expense.Description,
                Value = expense.Value,
                Date = expense.Date,
                Category = expense.Category.ToString(),
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while getting the expense.", ex);
        }
    }
}