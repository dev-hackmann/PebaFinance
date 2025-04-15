using PebaFinance.Domain.Enums;

namespace PebaFinance.Application.Exceptions;

public class EmailAlreadyRegisteredException : Exception
{
    public EmailAlreadyRegisteredException()
        : base($"Email already registered.")
    {
    }
}