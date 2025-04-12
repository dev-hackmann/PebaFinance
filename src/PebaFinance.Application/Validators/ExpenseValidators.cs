using FluentValidation;
using PebaFinance.Application.Commands;

namespace PebaFinance.Application.Validators;

public class BaseExpenseCommandValidator<T> : AbstractValidator<T> where T : IExpenseCommand
{
    public BaseExpenseCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Value).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
    }
}

public class CreateExpenseCommandValidator : BaseExpenseCommandValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
    }
}

public class UpdateExpenseCommandValidator : BaseExpenseCommandValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
    }
}