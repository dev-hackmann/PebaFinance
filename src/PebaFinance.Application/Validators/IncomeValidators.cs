using System;
using FluentValidation;
using PebaFinance.Application.Commands;

namespace PebaFinance.Application.Validators;

    public class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
    {
        public CreateIncomeCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Value).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
        }
    }