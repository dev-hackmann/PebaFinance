using System.Security.Claims;
using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PebaFinance.Application.Handlers.ExpensesHandlers;

public class DeleteExpensesCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpensesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteExpensesCommandHandler(IExpensesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return await _repository.DeleteAsync(request.Id, userId);
    }
}