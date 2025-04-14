using FluentValidation;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Queries;

namespace PebaFinance.Application.Validators;

public class BaseExpenseCommandValidator<T> : AbstractValidator<T> where T : IExpenseCommand
{
    public BaseExpenseCommandValidator()
    {
        RuleFor(command => command.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must be at most 200 characters.");

        RuleFor(command => command.Value)
            .GreaterThan(0).WithMessage("Value must be greater than zero.");

        RuleFor(command => command.Date)
            .NotEmpty().WithMessage("Date is required.");
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