using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Interfaces;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class DeleteIncomesCommandHandler : IRequestHandler<DeleteIncomeCommand, bool>
{
    private readonly IIncomesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteIncomesCommandHandler(IIncomesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return await _repository.DeleteAsync(request.Id, userId);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the income.", ex);
        }
    }
}