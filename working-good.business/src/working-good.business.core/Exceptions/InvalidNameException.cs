namespace working_good.business.core.Exceptions;

public sealed class InvalidNameException : CustomException
{
    public InvalidNameException() : base("Name can not be empty", "invalid_name")
    {
    }

    public InvalidNameException(string value) : base($"Name: {value} is invalid", "invalid_name")
    {
    }
}