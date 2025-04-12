using FluentValidation;
using PebaFinance.Application.Commands;

namespace PebaFinance.Application.Validators;

public class BaseIncomeCommandValidator<T> : AbstractValidator<T> where T : IIncomeCommand
{
    public BaseIncomeCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Value).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
    }
}

public class CreateIncomeCommandValidator : BaseIncomeCommandValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator()
    {
    }
}

public class UpdateIncomeCommandValidator : BaseIncomeCommandValidator<UpdateIncomeCommand>
{
    public UpdateIncomeCommandValidator()
    {
    }
}