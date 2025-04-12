using System;
using System.Globalization;

namespace PebaFinance.Application.Exceptions;

public class DuplicateDescriptionException : Exception
{
    public DuplicateDescriptionException(string description, DateTime date)
        : base($"An entity with the description '{description}' already exists for {DateTimeFormatInfo.CurrentInfo.GetMonthName(date.Month)} {date.Year}.")
    {
    }
}