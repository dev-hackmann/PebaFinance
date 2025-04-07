using FluentValidation;
using PebaFinance.Application.Commands;

namespace PebaFinance.Application.Validators;

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Value).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
    }
}