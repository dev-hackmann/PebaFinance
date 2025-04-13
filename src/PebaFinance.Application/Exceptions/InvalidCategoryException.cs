using PebaFinance.Domain.Enums;

namespace PebaFinance.Application.Exceptions;

public class InvalidCategoryException : Exception
{
    public InvalidCategoryException(string category)
        : base($"The category '{category}' is invalid. Valid categories are: {string.Join(", ", Enum.GetNames(typeof(ExpenseCategory)))}.")
    {
    }
}