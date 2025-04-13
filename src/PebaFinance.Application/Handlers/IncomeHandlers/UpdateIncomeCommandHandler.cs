using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;

namespace PebaFinance.Application.Handlers.IncomeHandlers;

public class UpdateIncomesCommandHandler : IRequestHandler<UpdateIncomeCommand, bool>
{
    private readonly IIncomesRepository _repository;

    public UpdateIncomesCommandHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _repository.GetByIdAsync(request.Id);
        if (income == null) return false;

        income.Description = request.Description;
        income.Value = request.Value;
        income.Date = request.Date;

        if (await _repository.ExistsByDescriptionInTheSameMonthAsync(income.Description, income.Date))
        {
            throw new DuplicateDescriptionException(income.Description, income.Date);
        }

        return await _repository.UpdateAsync(income);
    }
}