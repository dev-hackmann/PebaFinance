using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;

namespace PebaFinance.Application.Handlers.IncomeHandlers;

public class UpdateIncomesCommandHandler : IRequestHandler<UpdateIncomeCommand, bool>
{
    private readonly IIncomesRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateIncomesCommandHandler(IIncomesRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var income = await _repository.GetByIdAsync(request.Id, userId);
            if (income == null) return false;

            income.Description = request.Description;
            income.Value = request.Value;
            income.Date = request.Date;
            income.UserId = userId;

            if (await _repository.ExistsByDescriptionInTheSameMonthAsync(income.Description, income.Date, userId))
            {
                throw new DuplicateDescriptionException(income.Description, income.Date);
            }

            return await _repository.UpdateAsync(income);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the income.", ex);
        }
    }
}