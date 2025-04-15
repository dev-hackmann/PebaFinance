using FluentValidation;
using PebaFinance.Application.Queries;

public class AuthenticateUserQueryValidator : AbstractValidator<AuthenticateUserQuery>
{
    public AuthenticateUserQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100);
    }
}
