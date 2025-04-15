using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Enums;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class CreateExpensesCommandHandler : IRequestHandler<CreateExpenseCommand, int>
{
    private readonly IExpensesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateExpensesCommandHandler(IExpensesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {        
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (await _repository.ExistsByDescriptionInTheSameMonthAsync(request.Description, request.Date, userId))
            {
                throw new DuplicateDescriptionException(request.Description, request.Date);
            }

            ExpenseCategory? expenseCategory = null;
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                if (Enum.TryParse<ExpenseCategory>(request.Category, true, out var parsedCategory))
                {
                    expenseCategory = parsedCategory;
                }
                else
                {
                    throw new InvalidCategoryException(request.Category);
                }
            }

            var expense = new Expense(request.Description, request.Value, request.Date, userId, expenseCategory);

            return await _repository.AddAsync(expense);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the expense.", ex);
        }
    }
}
