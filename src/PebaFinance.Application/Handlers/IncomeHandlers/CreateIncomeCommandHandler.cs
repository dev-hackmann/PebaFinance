using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class CreateIncomesCommandHandler : IRequestHandler<CreateIncomeCommand, int>
{
    private readonly IIncomesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateIncomesCommandHandler(IIncomesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public async Task<int> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (await _repository.ExistsByDescriptionInTheSameMonthAsync(request.Description, request.Date, userId))
        {
            throw new DuplicateDescriptionException(request.Description, request.Date);
        }

        var income = new Income
        {
            Description = request.Description,
            Value = request.Value,
            Date = request.Date,
            UserId = userId
        };

        return await _repository.AddAsync(income);
    }
}
