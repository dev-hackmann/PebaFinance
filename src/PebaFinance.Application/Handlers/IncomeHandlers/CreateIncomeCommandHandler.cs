using MediatR;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.Application.Handlers.IncomesHandlers;

public class CreateIncomesCommandHandler : IRequestHandler<CreateIncomeCommand, int>
{
    private readonly IIncomesRepository _repository;

    public CreateIncomesCommandHandler(IIncomesRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByDescriptionInTheSameMonthAsync(request.Description, request.Date))
        {
            throw new DuplicateDescriptionException(request.Description, request.Date);
        }

        var income = new Income
        {
            Description = request.Description,
            Value = request.Value,
            Date = request.Date
        };

        return await _repository.AddAsync(income);
    }
}
