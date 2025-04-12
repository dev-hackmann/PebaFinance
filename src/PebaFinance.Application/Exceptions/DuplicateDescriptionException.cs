using System;

namespace PebaFinance.Application.Exceptions;

public class DuplicateDescriptionException : Exception
{
    public DuplicateDescriptionException(string description)
        : base($"An entity with the description '{description}' already exists.")
    {
    }
}
